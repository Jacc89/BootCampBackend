using System.Net;
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
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private readonly ILogger<ClienteController> _logger;
        private readonly IMapper _mapper;
      
        public ClienteController(ApplicationDbContext db, ILogger<ClienteController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _db = db; 
            _response = new ResponseDto();        

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {

             _logger.LogInformation("Listado de Clientes");
            var listas = await _db.TbCliente.ToListAsync();
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

            var Clie = await _db.TbCliente.FindAsync(id);
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
            var ClieExiste = await _db.TbCliente.FirstOrDefaultAsync
                                                ( c => c.Nombre.ToLower() == clienteDto.Nombre.ToLower());
            if (ClieExiste != null)
            {
                ModelState.AddModelError("NombreDuplicado", "Nombre del cliente ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }

            Cliente cliente = _mapper.Map<Cliente>(clienteDto);

            await _db.TbCliente.AddAsync(cliente);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetCliente", new {id= cliente.Id}, cliente); //status 201
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Cliente>>> PutCliente(int id, [FromBody] Cliente cliente)
        {
            if(id != cliente.Id){
                return BadRequest("Id de Cliente no  coincide");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ClieExiste = await _db.TbCliente.FirstOrDefaultAsync
                                                ( c => c.Nombre.ToLower() == cliente.Nombre.ToLower() && c.Id!= cliente.Id);
            if (ClieExiste != null)
            {
                ModelState.AddModelError("NombreDuplicado", "Nombre del cliente ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }
            _db.Update(cliente);
            await _db.SaveChangesAsync();
            return Ok(cliente);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            var cliente = await _db.TbCliente.FindAsync(id);
            if(cliente == null){
                return NotFound();
            }
            _db.TbCliente.Remove(cliente);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}