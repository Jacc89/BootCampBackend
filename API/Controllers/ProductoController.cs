using Core.Models;
using Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ProductoController(ApplicationDbContext db)
        {
            _db = db;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var listas = await _db.TbProducto.ToListAsync();
            return Ok(listas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProducto(int id)
        {
            var Prod = await _db.TbProducto.FindAsync(id);
            return Ok(Prod);
        }


    }
}