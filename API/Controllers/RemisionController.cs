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
        public RemisionController(ApplicationDbContext db)
        {
            _db = db;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Remision>>> GetRemisiones()
        {
            var listas = await _db.TbRemision.ToListAsync();
            return Ok(listas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetRemision(int id)
        {
            var Remi = await _db.TbRemision.FindAsync(id);
            return Ok(Remi);
        }
    }
}