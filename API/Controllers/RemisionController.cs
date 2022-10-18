using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Core.Dto;
using Core.Especificaciones;
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
        private ResponsePaginador _responsePaginador;
        private readonly ILogger<RemisionController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnidadTrabajo _unidadTrabajo;
        public RemisionController(IUnidadTrabajo unidadTrabajo, ILogger<RemisionController> logger, IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;
            _mapper = mapper;
            _logger = logger;
            _response = new ResponseDto();
            _responsePaginador = new ResponsePaginador();

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RemisionReadDto>>> GetRemisiones([FromQuery] Parametros parametros)
        
        {
             _logger.LogInformation("Listado de Remisiones");
            var listas = await _unidadTrabajo.Remision.ObtenerTodosPaginado( parametros,
                                                                             incluirPropiedades:"Empleado,Cliente,Producto",
                                                                             orderBy: r=>r.OrderBy(r=>r.NumRemision).ThenBy(r=>r.Cliente));
            // _db.TbRemision.Include(c=>c.Empleado).Include(c=>c.Cliente).Include(c=>c.Producto).ToListAsync();
            _responsePaginador.TotalPaginas = listas.MetaData.TotalPages;
            _responsePaginador.TotalRegistro = listas.MetaData.TotalCount;
            _responsePaginador.PageSize = listas.MetaData.PageSize;
            _responsePaginador.Resultado = _mapper.Map<IEnumerable<Remision>, IEnumerable<RemisionReadDto>>(listas);
            _responsePaginador.Mensaje = "Lista de remisiones";
            _responsePaginador.StatusCode = HttpStatusCode.OK;
            return Ok(_responsePaginador);
           
        }
        [HttpGet("{id}", Name = "GetRemision")]
        public async Task<ActionResult<IEnumerable<Remision>>> GetRemision(int id)
        {
            if (id ==  0)
            {
                _logger.LogError("Debe de Enviar el ID de remision");
                _response.Mensaje ="Debe de enviar el Id del Remision";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);                
            }
            var Remi = await _unidadTrabajo.Remision.ObtenerPrimero(c=>c.Id==id,incluirPropiedades:"Empleado,Cliente,Producto");
            // _db.TbRemision.Include(c=>c.Empleado).Include(c=>c.Cliente).Include(c=>c.Producto).FirstOrDefaultAsync(e => e.Id == id);
            if (Remi == null)
            {
                _logger.LogError("Remision No Existe!");
                _response.Mensaje ="Remision no existe!";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);   
                
            }
            _response.Resultado =  _mapper.Map<Remision, RemisionReadDto>(Remi);
            _response.Mensaje = "Datos de remisiones" + Remi.Id;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response); // status 200
        }

        [HttpGet]
        [Route("RemisionPorEmpleado/{EncargadoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RemisionReadDto>>> GetRemisionPorEmpleado(int EncargadoId)
        {
            _logger.LogInformation("Listado de remisiones por encargado");
            var lista = await _unidadTrabajo.Remision.ObtenerTodos(e=>e.EncargadoId == EncargadoId, incluirPropiedades:"Empleado");
            _response.Resultado = _mapper.Map<IEnumerable<Remision>, IEnumerable<RemisionReadDto>>(lista);
            _response.IsExitoso = true;
            _response.Mensaje = (" Listado de remisiones por encargado");
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Remision>>> PostRemision([FromBody] RemisionUpsertDto remisionDto)
        {
            if (remisionDto == null)
            {
                _response.Mensaje =" Informacion Incorrecta del remision";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
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
                // ModelState.AddModelError("RemisionDuplicado", "Numero de remision ya existe"); // model state personaliazado
                _response.IsExitoso = false;
                _response.Mensaje = "Nª Remision ya existe";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            Remision remision = _mapper.Map<Remision>(remisionDto);
            await _unidadTrabajo.Remision.Agregar(remision);
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = false;
            _response.Mensaje = "Remision Guardado con exito";
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetRemision", new {id= remision.Id}, _response); //status 201

        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Remision>>> PutRemision(int id, [FromBody] RemisionUpsertDto remisionDto)
        {
            if(id != remisionDto.Id){
                _response.IsExitoso = false;
                _response.Mensaje = "Id de Remision no  coincide";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var remiExiste = await _unidadTrabajo.Remision.ObtenerPrimero
                                                ( r => r.NumRemision == remisionDto.NumRemision && r.Id!= remisionDto.Id);
            if (remiExiste != null)
            {
                // ModelState.AddModelError("RemisionDuplicado", "Numero de remision ya existe"); // model state personaliazado
                _response.IsExitoso = false;
                _response.Mensaje = "Numero de remision ya existe";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            Remision remision = _mapper.Map<Remision>(remisionDto);
            _unidadTrabajo.Remision.Actualizar(remision);
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = false;
            _response.Mensaje = "Remision guardada con exito";
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRemision(int id)
        {
            var remision = await _unidadTrabajo.Remision.ObtenerPrimero(c=>c.Id==id);
            if(remision == null){
                _response.IsExitoso = false;
                _response.Mensaje = "Remision No existe";
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _unidadTrabajo.Remision.Remover(remision);
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = false;
            _response.Mensaje = "Remision Eliminada";
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }


    }
}