using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;

namespace Infraestructura.Data.Repositorio
{
    public class RemisionRepositorio : Repositorio<Remision>, IRemisionRepositorio
    {
        private readonly ApplicationDbContext _db;
        public RemisionRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Remision remision)
        {
            var remisionDB = _db.TbRemision.FirstOrDefault(c=>c.Id == remision.Id);
            if (remisionDB != null)
            {
                remisionDB.NumRemision = remision.NumRemision;
                remisionDB.Fecha = remision.Fecha;
                remisionDB.EncargadoId = remision.EncargadoId;
                remisionDB.ClienteId = remision.ClienteId;
                remisionDB.Cantidad = remision.Cantidad;
                remisionDB.Presentacion = remision.Presentacion;
                remisionDB.ProductoId = remision.ProductoId;
                remisionDB.Observaciones = remision.Observaciones;
                _db.SaveChanges();
                
            }
        }
    }
}