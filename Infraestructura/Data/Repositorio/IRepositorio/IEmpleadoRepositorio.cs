using Core.Models;

namespace Infraestructura.Data.Repositorio.IRepositorio
{
    public interface IEmpleadoRepositorio: IRepositorio<Empleado>
    {
        void Actualizar(Empleado empleado);
    }
}