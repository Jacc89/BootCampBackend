using Core.Models;
using Infraestructura.Data.Repositorio.IRepositorio;

namespace Infraestructura.Data.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _db;
        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Producto producto)
        {
            var productoDB = _db.TbProducto.FirstOrDefault(c=>c.Id == producto.Id);
            if (productoDB != null)
            {
                productoDB.Nombre = producto.Nombre;
                productoDB.Caracteristica = producto.Caracteristica;
                _db.SaveChanges();
                
            }
        }
    }
}