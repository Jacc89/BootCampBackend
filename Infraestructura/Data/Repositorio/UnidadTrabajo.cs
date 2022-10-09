using Infraestructura.Data.Repositorio.IRepositorio;


namespace Infraestructura.Data.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        
        public IClienteRepositorio Cliente  {get; private set;}
        public IEmpleadoRepositorio Empleado  {get; private set;}
        public IProductoRepositorio Producto  {get; private set;}
        public IRemisionRepositorio Remision  {get; private set;}


        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Cliente = new ClienteRepositorio(db);
            Empleado = new EmpleadoRepositorio(db);
            Producto = new ProductoRepositorio(db);
            Remision = new RemisionRepositorio(db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}