using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;

namespace PuntoDeVenta.AccesoDatos.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Obtener(Expression<Func<TEntity, bool>> Filtro);
        Task<TEntity> Crear(TEntity Entidad);

        Task<bool> Editar(TEntity Entidad);
        Task<bool> Eliminar(TEntity Entidad);

        Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>>? Filtro = null);
    }
}
