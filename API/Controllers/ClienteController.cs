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
      
        public ClienteController(ApplicationDbContext db)
        {
            _db = db; 
            _response = new ResponseDto();        

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var listas = await _db.TbCliente.ToListAsync();
            _response.Resultado =  listas;
            _response.Mensaje = " lista de clientes";
            return Ok(_response);
        }
        [HttpGet("{id}", Name = "GetCliente")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente(int id)
        {
            var Clie = await _db.TbCliente.FindAsync(id);
            _response.Resultado =  Clie;
            _response.Mensaje = " Datos de cliente" + Clie.Id;
            return Ok(_response);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Cliente>>> PostCliente([FromBody] Cliente cliente)
        {
            await _db.TbCliente.AddAsync(cliente);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetCliente", new {id= cliente.Id}, cliente); //status 201
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> PutCliente(int id, [FromBody] Cliente cliente)
        {
            if(id != cliente.Id){
                return BadRequest("Id de Cliente no  coincide");
            }
            _db.Update(cliente);
            await _db.SaveChangesAsync();
            return Ok(cliente);
        }
        [HttpDelete("{id}")]
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