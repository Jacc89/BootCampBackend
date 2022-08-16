using Core.Dto;
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
        private ResponseDto _response;
       
         public EmpleadoController(ApplicationDbContext db)
         {
            _db = db;
            _response = new ResponseDto();
                       

         }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            var listas = await _db.TbEmpleado.ToListAsync();
            _response.Resultado =  listas;
            _response.Mensaje = "Lista de empleados";
            return Ok(_response);
        }
        [HttpGet("{id}", Name = "GetEmpleado")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleado(int id)
        {
            var Empl = await _db.TbEmpleado.FindAsync(id);
            _response.Resultado =  Empl;
            _response.Mensaje = "Datos de empleado";
            return Ok(_response);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Empleado>>> PostEmpleado([FromBody] Empleado empleado)
        {
            await _db.TbEmpleado.AddAsync(empleado);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetEmpleado", new {id= empleado.Id}, empleado); //status 201
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Empleado>>> PutEmpleado(int id, [FromBody] Empleado empleado)
        {
            if(id != empleado.Id){
                return BadRequest("Id de empleado no  coincide");
            }
            _db.Update(empleado);
            await _db.SaveChangesAsync();
            return Ok(empleado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmpleado(int id)
        {
            var empleado = await _db.TbEmpleado.FindAsync(id);
            if(empleado == null){
                return NotFound();
            }
            _db.TbEmpleado.Remove(empleado);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    
    }
}