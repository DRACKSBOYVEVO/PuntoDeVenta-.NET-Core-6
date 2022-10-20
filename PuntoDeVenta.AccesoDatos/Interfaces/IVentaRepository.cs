using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuntoDeVenta.Entity;

namespace PuntoDeVenta.AccesoDatos.Interfaces
{
    public interface IVentaRepository : IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta Entidad);
        Task<List<DetalleVenta>> Reporte(DateTime FechaDeInicio, DateTime FechaDeFin);
    }
}
