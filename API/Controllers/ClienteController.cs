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
    public class ClienteController : ControllerBase
    {
       
        private ResponseDto _response;
        private readonly ILogger<ClienteController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnidadTrabajo _unidadTrabajo;
      
        public ClienteController(IUnidadTrabajo unidadTrabajo, ILogger<ClienteController> logger, IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;
            _mapper = mapper;
            _logger = logger;
            _response = new ResponseDto();        

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {

             _logger.LogInformation("Listado de Clientes");
            var listas = await _unidadTrabajo.Cliente.ObtenerTodos();
            _response.Resultado =  listas;
            _response.Mensaje = " lista de clientes";
            return Ok(_response);
        }
        [HttpGet("{id}", Name = "GetCliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente(int id)
        {
            if (id ==  0)
            {
                 _logger.LogError("Debe de Enviar el ID de cliente");
                _response.Mensaje ="Debe de enviar el Id del Cliente";
                _response.IsExitoso = false;
                return BadRequest(_response);                
            }

            var Clie = await _unidadTrabajo.Cliente.ObtenerPrimero(c=>c.Id ==id);
            if (Clie == null)
            {
                _logger.LogError("Cliente No Existe!");
                _response.Mensaje ="Cliente no existe!";
                _response.IsExitoso = false;
                return NotFound(_response);   
                
            }

            _response.Resultado =  Clie;
            _response.Mensaje = " Datos de cliente" + Clie.Id;
            return Ok(_response);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Cliente>>> PostCliente([FromBody] ClienteDto clienteDto)
        {
            if (clienteDto == null)
            {
                _response.Mensaje =" Informacion Incorrecta del Cliente";
                _response.IsExitoso = false;
                return BadRequest(_response);   
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ClieExiste = await _unidadTrabajo.Cliente.ObtenerPrimero
                                                ( c => c.Nombre.ToLower() == clienteDto.Nombre.ToLower());
            if (ClieExiste != null)
            {
                ModelState.AddModelError("NombreDuplicado", "Nombre del cliente ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }

            Cliente cliente = _mapper.Map<Cliente>(clienteDto);

            await _unidadTrabajo.Cliente.Agregar(cliente);
            await _unidadTrabajo.Guardar();
            return CreatedAtRoute("GetCliente", new {id= cliente.Id}, cliente); //status 201
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Cliente>>> PutCliente(int id, [FromBody] ClienteDto clienteDto)
        {
            if(id != clienteDto.Id){
                return BadRequest("Id de Cliente no  coincide");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ClieExiste = await _unidadTrabajo.Cliente.ObtenerPrimero
                                                ( c => c.Nombre.ToLower() == clienteDto.Nombre.ToLower() && c.Id!= clienteDto.Id);
            if (ClieExiste != null)
            {
                ModelState.AddModelError("NombreDuplicado", "Nombre del cliente ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }
            Cliente cliente = _mapper.Map<Cliente>(clienteDto);
            _unidadTrabajo.Cliente.Actualizar(cliente);
            await _unidadTrabajo.Guardar();
            return Ok(cliente);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            var cliente = await _unidadTrabajo.Cliente.ObtenerPrimero(c=>c.Id==id);
            if(cliente == null){
                return NotFound();
            }
            _unidadTrabajo.Cliente.Remover(cliente);
            await _unidadTrabajo.Guardar();
            return NoContent();
        }

    }
}