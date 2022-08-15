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
      
        public ClienteController(ApplicationDbContext db)
        {
            _db = db;         

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var listas = await _db.TbCliente.ToListAsync();
            return Ok(listas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente(int id)
        {
            var Clie = await _db.TbCliente.FindAsync(id);
            return Ok(Clie);
        }

    }
}