using Core.Dto;
using Core.Models;
using Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemisionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        public RemisionController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ResponseDto();

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Remision>>> GetRemisiones()
        {
            var listas = await _db.TbRemision.ToListAsync();
            _response.Resultado =  listas;
            _response.Mensaje = "Lista de remisiones";
            return Ok(_response);
           
        }
        [HttpGet("{id}", Name = "GetRemision")]
        public async Task<ActionResult<IEnumerable<Remision>>> GetRemision(int id)
        {
            if (id ==  0)
            {
                _response.Mensaje ="Debe de enviar el Id del Remision";
                _response.IsExitoso = false;
                return BadRequest(_response);                
            }
            var Remi = await _db.TbRemision.FindAsync(id);
            if (Remi == null)
            {
                _response.Mensaje ="Remision no existe!";
                _response.IsExitoso = false;
                return BadRequest(_response);   
                
            }
            _response.Resultado =  Remi;
            _response.Mensaje = "Datos de remisiones";
            return Ok(_response); // status 200
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Remision>>> PostRemision([FromBody] Remision remision)
        {
            await _db.TbRemision.AddAsync(remision);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetRemision", new {id= remision.Id}, remision); //status 201

        }
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Remision>>> PutRemision(int id, [FromBody] Remision remision)
        {
            if(id != remision.Id){
                return BadRequest("Id de Remision no  coincide");
            }
            _db.Update(remision);
            await _db.SaveChangesAsync();
            return Ok(remision);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRemision(int id)
        {
            var remision = await _db.TbRemision.FindAsync(id);
            if(remision == null){
                return NotFound();
            }
            _db.TbRemision.Remove(remision);
            await _db.SaveChangesAsync();
            return NoContent();
        }


    }
}