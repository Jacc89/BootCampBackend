using Core.Models;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IClienteRepositorio : IRepositorio<Cliente>
    {
        void Actualizar(Cliente cliente);
        
    }
}