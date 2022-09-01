using AutoMapper;
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
        private readonly ILogger<EmpleadoController> _logger;
        private readonly IMapper _mapper;
       
         public EmpleadoController(ApplicationDbContext db, ILogger<EmpleadoController> logger, IMapper mapper)
         {
            _mapper = mapper;
            _logger = logger;
            _db = db;
            _response = new ResponseDto();
                       

         }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
             _logger.LogInformation("Listado de Empleados");
            var listas = await _db.TbEmpleado.ToListAsync();
            _response.Resultado =  listas;
            _response.Mensaje = "Lista de empleados";
            return Ok(_response);
        }
        [HttpGet("{id}", Name = "GetEmpleado")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleado(int id)
        {
            if (id ==  0)
            {
                 _logger.LogError("Debe de Enviar el ID del empleado");
                _response.Mensaje ="Debe de enviar el Id del Empleado";
                _response.IsExitoso = false;
                return BadRequest(_response);                
            }
            var Empl = await _db.TbEmpleado.FindAsync(id);

             if (Empl == null)
            {
                _logger.LogError("Empleado No Existe!");
                _response.Mensaje ="Empleado no existe!";
                _response.IsExitoso = false;
                return NotFound(_response);   
                
            }
            _response.Resultado =  Empl;
            _response.Mensaje = "Datos de empleado";
            return Ok(_response);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Empleado>>> PostEmpleado([FromBody] EmpleadoDto empleadoDto)
        {
            if (empleadoDto == null)
            {
                _response.Mensaje =" Informacion Incorrecta del empleado";
                _response.IsExitoso = false;
                return BadRequest(_response);   
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var empleExiste = await _db.TbEmpleado.FirstOrDefaultAsync
                                                ( e => e.Id == empleadoDto.Id);
            if (empleExiste != null)
            {
                ModelState.AddModelError("cedulaDuplicado", "Numero de identidad ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }

            Empleado empleado = _mapper.Map<Empleado>(empleadoDto);
            await _db.TbEmpleado.AddAsync(empleado);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetEmpleado", new {id= empleado.Id}, empleado); //status 201
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Empleado>>> PutEmpleado(int id, [FromBody] EmpleadoDto empleadoDto)
        {
            if(id != empleadoDto.Id){
                return BadRequest("Id de empleado no  coincide");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var empleExiste = await _db.TbEmpleado.FirstOrDefaultAsync
                                                ( e =>e.Nombres.ToLower() == empleadoDto.Nombres.ToLower() 
                                                && e.Id == empleadoDto.Id);
            if (empleExiste != null)
            {
                ModelState.AddModelError("cedulaDuplicado", "Numero de identidad ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }
            Empleado empleado = _mapper.Map<Empleado>(empleadoDto);
            _db.Update(empleado);
            await _db.SaveChangesAsync();
            return Ok(empleado);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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