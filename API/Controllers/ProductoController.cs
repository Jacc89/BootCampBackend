using Core.Dto;
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
        private ResponseDto _response;
        public ProductoController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ResponseDto(); 

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var listas = await _db.TbProducto.ToListAsync();
            _response.Resultado =  listas;
            _response.Mensaje = "Lista de productos";
            return Ok(_response);
        }
        [HttpGet("{id}", Name = "GetProducto")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProducto(int id)
        {
            if (id ==  0)
            {
                _response.Mensaje ="Debe de enviar el Id del Producto";
                _response.IsExitoso = false;
                return BadRequest(_response);                
            }

            var Prod = await _db.TbProducto.FindAsync(id);
            if (Prod == null)
            {
                _response.Mensaje ="Producto no existe!";
                _response.IsExitoso = false;
                return NotFound(_response);   
                
            }
             _response.Resultado =  Prod;
            _response.Mensaje = "Datos del producto";
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Producto>>> PostProducto([FromBody] Producto producto)
        {
            await _db.TbProducto.AddAsync(producto);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetProducto", new {id= producto.Id}, producto); //status 201
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<Producto>>> PutProducto(int id, [FromBody] Producto producto)
        {
            if(id != producto.Id){
                return BadRequest("Id de Producto no  coincide");
            }
            _db.Update(producto);
            await _db.SaveChangesAsync();
            return Ok(producto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProducto(int id)
        {
            var producto = await _db.TbProducto.FindAsync(id);
            if(producto == null){
                return NotFound();
            }
            _db.TbProducto.Remove(producto);
            await _db.SaveChangesAsync();
            return NoContent();
        }


    }
}