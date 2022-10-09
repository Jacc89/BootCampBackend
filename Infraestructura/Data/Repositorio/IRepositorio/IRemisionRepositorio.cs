using Core.Models;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IRemisionRepositorio : IRepositorio<Remision>
    {
        void Actualizar(Remision remision);
    }
}