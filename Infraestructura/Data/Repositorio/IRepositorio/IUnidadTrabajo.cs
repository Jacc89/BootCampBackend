namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        IClienteRepositorio Cliente {get; }
        IEmpleadoRepositorio Empleado {get; }
        IProductoRepositorio Producto {get; }
        IRemisionRepositorio Remision {get; }
        Task Guardar();
    }
}