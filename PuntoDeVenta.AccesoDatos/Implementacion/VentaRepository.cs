using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuntoDeVenta.AccesoDatos.DBContext;
using PuntoDeVenta.AccesoDatos.Interfaces;
using Microsoft.EntityFrameworkCore;
using PuntoDeVenta.Entity;
using System.Linq.Expressions;

namespace PuntoDeVenta.AccesoDatos.Implementacion
{
    public class IVentaRepository : IGenericRepository<Venta>, Interfaces.IVentaRepository
    {

        private readonly DB_PROYECTOContext DB_Context;

        //public IVentaRepository(DB_PROYECTOContext DBContext) : base(DBContext)
        //{
        //    DB_Context = DBContext;
        //}


        public Task<Venta> Obtener(Expression<Func<Venta, bool>> Filtro)
        {
            throw new NotImplementedException();
        }

        public Task<Venta> Crear(Venta Entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Editar(Venta Entidad)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(Venta Entidad)
        {
            throw new NotImplementedException();
        }

        public async Task<Venta> Registrar(Venta Entidad)
        {
            Venta VentaGenerada = new();

            using var Transaccion = DB_Context.Database.BeginTransaction();

            try
            {
                foreach (DetalleVenta detalleVenta in Entidad.DetalleVenta)
                {
                    Producto ProductoEncontrado = DB_Context.Productos.Where(producto => producto.IdProducto == detalleVenta.IdProducto).First();
                    ProductoEncontrado.Stock -= detalleVenta.Cantidad;
                    DB_Context.Productos.Update(ProductoEncontrado);
                }
                await DB_Context.SaveChangesAsync();

                NumeroCorrelativo correlativo = DB_Context.NumeroCorrelativos.Where(numero => numero.Gestion == "venta").First();
                correlativo.UltimoNumero++;
                correlativo.FechaActualizacion = DateTime.Now;

                DB_Context.NumeroCorrelativos.Update(correlativo);
                await DB_Context.SaveChangesAsync();

                string CantidadInicial = string.Concat(Enumerable.Repeat("0", correlativo.CantidadDigitos.Value));
                string NumeroVenta = CantidadInicial + correlativo.UltimoNumero.ToString();

                NumeroVenta = NumeroVenta.Substring(NumeroVenta.Length - correlativo.CantidadDigitos.Value, correlativo.CantidadDigitos.Value);

                Entidad.NumeroVenta = NumeroVenta;

                await DB_Context.Venta.AddAsync(Entidad);
                await DB_Context.SaveChangesAsync();

                VentaGenerada = Entidad;

                Transaccion.Commit();
            }
            catch (Exception)
            {
                Transaccion.Rollback();
                throw;
            }

            return VentaGenerada;
        }

        public async Task<List<DetalleVenta>> Reporte(DateTime FechaDeInicio, DateTime FechaDeFin)
        {
            List<DetalleVenta> ListaResumen = await DB_Context.DetalleVenta
                .Include(Tableventa => Tableventa.IdVentaNavigation)
                .ThenInclude(usuario => usuario.IdUsuarioNavigation)
                .Include(Tableventa => Tableventa.IdVentaNavigation)
                .ThenInclude(TipoDetalleVenta => TipoDetalleVenta.IdTipoDocumentoVentaNavigation)
                .Where(DB_Venta => DB_Venta.IdVentaNavigation.FechaRegistro.Value.Date >= FechaDeFin.Date &&
                DB_Venta.IdVentaNavigation.FechaRegistro.Value.Date <= FechaDeFin.Date).ToListAsync();

            return ListaResumen;
        }

        public Task<IQueryable<Venta>> Consultar(Expression<Func<Venta, bool>>? Filtro = null)
        {
            throw new NotImplementedException();
        }
    }
}
