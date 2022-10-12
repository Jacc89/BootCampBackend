using System.Net;
using AutoMapper;
using Core.Dto;
using Core.Models;
using Infraestructura.Data;
using Infraestructura.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private ResponseDto _response;
        private readonly ILogger<EmpleadoController> _logger;
        private readonly IMapper _mapper;
        
        private readonly IUnidadTrabajo _unidadTrabajo;
       
         public EmpleadoController(IUnidadTrabajo unidadTrabajo, ILogger<EmpleadoController> logger, IMapper mapper)
         {
            _unidadTrabajo = unidadTrabajo;
            
            _mapper = mapper;
            _logger = logger;
            _response = new ResponseDto();
                       

         }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
             _logger.LogInformation("Listado de Empleados");
            var listas = await _unidadTrabajo.Cliente.ObtenerTodos();
            _response.Resultado =  listas;
            _response.Mensaje = "Lista de empleados";
            _response.StatusCode = HttpStatusCode.OK;
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
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);                
            }
            var Empl = await _unidadTrabajo.Empleado.ObtenerPrimero(c=>c.Id==id);

             if (Empl == null)
            {
                _logger.LogError("Empleado No Existe!");
                _response.Mensaje ="Empleado no existe!";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);   
                
            }
            _response.Resultado =  Empl;
            _response.Mensaje = "Datos de empleado" + Empl.Id;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Empleado>>> PostEmpleado([FromBody] EmpleadoDto empleadoDto)
        {
            if (empleadoDto == null)
            {
                _response.Mensaje =" Informacion Incorrecta del empleado";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);   
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var empleExiste = await _unidadTrabajo.Empleado.ObtenerPrimero
                                                ( e => e.Id == empleadoDto.Id);
            if (empleExiste != null)
            {
                // ModelState.AddModelError("cedulaDuplicado", "Numero de identidad ya existe"); // model state personaliazado
                _response.IsExitoso = false;
                _response.Mensaje = "Nombre del empleado ya existe";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Empleado empleado = _mapper.Map<Empleado>(empleadoDto);
            await _unidadTrabajo.Empleado.Agregar(empleado);
            await _unidadTrabajo.Guardar();
             _response.IsExitoso = false;
            _response.Mensaje = "Empleado Guardado con exito";
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetEmpleado", new {id= empleado.Id}, empleado); //status 201
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Empleado>>> PutEmpleado(int id, [FromBody] EmpleadoDto empleadoDto)
        {
            if(id != empleadoDto.Id){
                _response.IsExitoso = false;
                _response.Mensaje = "Id de empleado no  coincide";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
               
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var empleExiste = await _unidadTrabajo.Empleado.ObtenerPrimero
                                                ( e =>e.Nombres.ToLower() == empleadoDto.Nombres.ToLower() 
                                                && e.Id == empleadoDto.Id);
            if (empleExiste != null)
            {
                // ModelState.AddModelError("cedulaDuplicado", "Numero de identidad ya existe"); // model state personaliazado
                _response.IsExitoso = false;
                _response.Mensaje = "Nombre del Empleado ya existe";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            Empleado empleado = _mapper.Map<Empleado>(empleadoDto);
            _unidadTrabajo.Empleado.Actualizar(empleado);
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = false;
            _response.Mensaje = "Empleado Actualizado";
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteEmpleado(int id)
        {
            var empleado = await _unidadTrabajo.Empleado.ObtenerPrimero(c=>c.Id==id);
            if(empleado == null){
                 _response.IsExitoso = false;
                _response.Mensaje = "Empleado No existe";
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _unidadTrabajo.Empleado.Remover(empleado);
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = false;
            _response.Mensaje = "Empleado Eliminado";
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
            
        }

    
    }
}