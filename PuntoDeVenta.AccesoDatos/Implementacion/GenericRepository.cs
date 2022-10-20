using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuntoDeVenta.AccesoDatos.DBContext;
using PuntoDeVenta.AccesoDatos.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PuntoDeVenta.AccesoDatos.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DB_PROYECTOContext DB_Context;

        public GenericRepository(DB_PROYECTOContext DBContext)
        {
            DB_Context = DBContext;
        }

        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> Filtro)
        {
            try
            {
                TEntity Entidad = await DB_Context.Set<TEntity> ().FirstOrDefaultAsync(Filtro);
                return Entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TEntity> Crear(TEntity Entidad)
        {
            try
            {
                DB_Context.Set<TEntity> ().Add(Entidad);
                await DB_Context.SaveChangesAsync();
                return Entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TEntity Entidad)
        {
            try
            {
                DB_Context.Update(Entidad);
                await DB_Context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(TEntity Entidad)
        {
            try
            {
                DB_Context.Remove(Entidad);
                await DB_Context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>>? Filtro = null)
        {
            IQueryable<TEntity> QueryEntidad = Filtro == null ? DB_Context.Set<TEntity>() : DB_Context.Set<TEntity>().Where(Filtro);
            return QueryEntidad;
        }   
    }
}
