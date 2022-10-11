using System.Collections.Generic;
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
    public class RemisionController : ControllerBase
    {
        private ResponseDto _response;
        private readonly ILogger<RemisionController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnidadTrabajo _unidadTrabajo;
        public RemisionController(IUnidadTrabajo unidadTrabajo, ILogger<RemisionController> logger, IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;
            _mapper = mapper;
            _logger = logger;
            _response = new ResponseDto();

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RemisionDto>>> GetRemisiones()
        {
             _logger.LogInformation("Listado de Remisiones");
            var listas = await _unidadTrabajo.Remision.ObtenerTodos(incluirPropiedades:"Empleado, Cliente, Producto");
            // _db.TbRemision.Include(c=>c.Empleado).Include(c=>c.Cliente).Include(c=>c.Producto).ToListAsync();
            _response.Resultado = _mapper.Map<IEnumerable<Remision>, IEnumerable<RemisionDto>>(listas);
            _response.Mensaje = "Lista de remisiones";
            return Ok(_response);
           
        }
        [HttpGet("{id}", Name = "GetRemision")]
        public async Task<ActionResult<IEnumerable<Remision>>> GetRemision(int id)
        {
            if (id ==  0)
            {
                _logger.LogError("Debe de Enviar el ID de remision");
                _response.Mensaje ="Debe de enviar el Id del Remision";
                _response.IsExitoso = false;
                return BadRequest(_response);                
            }
            var Remi = await _unidadTrabajo.Remision.ObtenerPrimero(c=>c.Id==id,incluirPropiedades:"Empleado,Cliente,Producto");
            // _db.TbRemision.Include(c=>c.Empleado).Include(c=>c.Cliente).Include(c=>c.Producto).FirstOrDefaultAsync(e => e.Id == id);
            if (Remi == null)
            {
                _logger.LogError("Remision No Existe!");
                _response.Mensaje ="Remision no existe!";
                _response.IsExitoso = false;
                return NotFound(_response);   
                
            }
            _response.Resultado =  _mapper.Map<Remision, RemisionDto>(Remi);
            _response.Mensaje = "Datos de remisiones";
            return Ok(_response); // status 200
        }

        [HttpGet]
        [Route("RemisionPorEmpleado/{EncargadoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RemisionDto>>> GetRemisionPorEmpleado(int EncargadoId)
        {
            _logger.LogInformation("Listado de remisiones por encargado");
            var lista = await _unidadTrabajo.Remision.ObtenerTodos(e=>e.EncargadoId == EncargadoId, incluirPropiedades:"Enc");
            _response.Resultado = _mapper.Map<IEnumerable<Remision>, IEnumerable<RemisionDto>>(lista);
            _response.IsExitoso = true;
            _response.Mensaje = (" Listado de remisiones por encargado");
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Remision>>> PostRemision([FromBody] RemisionDto remisionDto)
        {
            if (remisionDto == null)
            {
                _response.Mensaje =" Informacion Incorrecta del remision";
                _response.IsExitoso = false;
                return BadRequest(_response);   
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var remiExiste = await  _unidadTrabajo.Remision.ObtenerPrimero
                                                ( r => r.NumRemision == remisionDto.NumRemision);
            if (remiExiste != null)
            {
                ModelState.AddModelError("RemisionDuplicado", "Numero de remision ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }
            Remision remision = _mapper.Map<Remision>(remisionDto);
            await _unidadTrabajo.Remision.Agregar(remision);
            await _unidadTrabajo.Guardar();
            return CreatedAtRoute("GetRemision", new {id= remision.Id}, remision); //status 201

        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Remision>>> PutRemision(int id, [FromBody] RemisionDto remisionDto)
        {
            if(id != remisionDto.Id){
                return BadRequest("Id de Remision no  coincide");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var remiExiste = await _unidadTrabajo.Remision.ObtenerPrimero
                                                ( r => r.NumRemision == remisionDto.NumRemision && r.Id!= remisionDto.Id);
            if (remiExiste != null)
            {
                ModelState.AddModelError("RemisionDuplicado", "Numero de remision ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }
            Remision remision = _mapper.Map<Remision>(remisionDto);
            _unidadTrabajo.Remision.Actualizar(remision);
            await _unidadTrabajo.Guardar();
            return Ok(remision);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRemision(int id)
        {
            var remision = await _unidadTrabajo.Remision.ObtenerPrimero(c=>c.Id==id);
            if(remision == null){
                return NotFound();
            }
            _unidadTrabajo.Remision.Remover(remision);
            await _unidadTrabajo.Guardar();
            return NoContent();
        }


    }
}