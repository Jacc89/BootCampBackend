using Core.Models;
using Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        
        private readonly ApplicationDbContext _db;
       
         public EmpleadoController(ApplicationDbContext db)
         {
            _db = db;
                       

         }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            var listas = await _db.TbEmpleado.ToListAsync();
            return Ok(listas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleado(int id)
        {
            var Empl = await _db.TbEmpleado.FindAsync(id);
            return Ok(Empl);
        }

    
    }
}