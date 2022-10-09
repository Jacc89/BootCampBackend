using System;
using System.Linq.Expressions;
namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerTodos(
            Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy =null,
            string incluirPropiedades = null // includes propierdades de otras tablas

        );
        Task<T> ObtenerPrimero(
            Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null // includes propierdades de otras tablas

        );

        Task Agregar( T Models);

        void Remover(T Models);

    }
}