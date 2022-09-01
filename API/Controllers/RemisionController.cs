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
        private readonly ILogger<RemisionController> _logger;
        public RemisionController(ApplicationDbContext db, ILogger<RemisionController> logger)
        {
            _logger = logger;
            _db = db;
            _response = new ResponseDto();

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Remision>>> GetRemisiones()
        {
             _logger.LogInformation("Listado de Remisiones");
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
                _logger.LogError("Debe de Enviar el ID de remision");
                _response.Mensaje ="Debe de enviar el Id del Remision";
                _response.IsExitoso = false;
                return BadRequest(_response);                
            }
            var Remi = await _db.TbRemision.FindAsync(id);
            if (Remi == null)
            {
                _logger.LogError("Remision No Existe!");
                _response.Mensaje ="Remision no existe!";
                _response.IsExitoso = false;
                return NotFound(_response);   
                
            }
            _response.Resultado =  Remi;
            _response.Mensaje = "Datos de remisiones";
            return Ok(_response); // status 200
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Remision>>> PostRemision([FromBody] Remision remision)
        {
            if (remision == null)
            {
                _response.Mensaje =" Informacion Incorrecta del remision";
                _response.IsExitoso = false;
                return BadRequest(_response);   
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var remiExiste = await _db.TbRemision.FirstOrDefaultAsync
                                                ( r => r.NumRemision == remision.NumRemision);
            if (remiExiste != null)
            {
                ModelState.AddModelError("RemisionDuplicado", "Numero de remision ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }
            await _db.TbRemision.AddAsync(remision);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetRemision", new {id= remision.Id}, remision); //status 201

        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Remision>>> PutRemision(int id, [FromBody] Remision remision)
        {
            if(id != remision.Id){
                return BadRequest("Id de Remision no  coincide");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var remiExiste = await _db.TbRemision.FirstOrDefaultAsync
                                                ( r => r.NumRemision == remision.NumRemision && r.Id!= remision.Id);
            if (remiExiste != null)
            {
                ModelState.AddModelError("RemisionDuplicado", "Numero de remision ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }
            _db.Update(remision);
            await _db.SaveChangesAsync();
            return Ok(remision);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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