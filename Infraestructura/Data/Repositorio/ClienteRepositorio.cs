using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;

namespace Infraestructura.Data.Repositorio
{
    public class ClienteRepositorio : Repositorio<Cliente>, IClienteRepositorio
    {
        private readonly ApplicationDbContext _db;
        public ClienteRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Cliente cliente)
        {
            var clienteDB = _db.TbCliente.FirstOrDefault(c=>c.Id == cliente.Id);
            if (clienteDB != null)
            {
                clienteDB.Nombre = cliente.Nombre;
                clienteDB.Direccion = cliente.Direccion;
                clienteDB.Telefono = cliente.Telefono;
                _db.SaveChanges();
                
            }
        }
    }
}