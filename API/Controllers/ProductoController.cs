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
        private readonly ILogger<ProductoController> _logger;
        public ProductoController(ApplicationDbContext db, ILogger<ProductoController> logger)
        {
            _logger = logger;
            _db = db;
            _response = new ResponseDto(); 

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
             _logger.LogInformation("Listado de Productos");
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
                 _logger.LogError("Debe de Enviar el ID de producto");
                _response.Mensaje ="Debe de enviar el Id del Producto";
                _response.IsExitoso = false;
                return BadRequest(_response);                
            }

            var Prod = await _db.TbProducto.FindAsync(id);
            if (Prod == null)
            {
                _logger.LogError("Producto No Existe!");
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
            if (producto == null)
            {
                _response.Mensaje =" Informacion Incorrecta del producto";
                _response.IsExitoso = false;
                return BadRequest(_response);   
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ProdExiste = await _db.TbProducto.FirstOrDefaultAsync
                                                ( p => p.Nombre.ToLower() == producto.Nombre.ToLower());
            if (ProdExiste != null)
            {
                ModelState.AddModelError("NombreDuplicado", "Nombre de producto ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }

            await _db.TbProducto.AddAsync(producto);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetProducto", new {id= producto.Id}, producto); //status 201
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Producto>>> PutProducto(int id, [FromBody] Producto producto)
        {
            if(id != producto.Id){
                return BadRequest("Id de Producto no  coincide");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ProdExiste = await _db.TbProducto.FirstOrDefaultAsync
                                                ( p => p.Nombre.ToLower() == producto.Nombre.ToLower()
                                                && p.Id!= producto.Id);
            if (ProdExiste != null)
            {
                ModelState.AddModelError("NombreDuplicado", "Nombre de producto ya existe"); // model state personaliazado
                return BadRequest(ModelState);
            }
            _db.Update(producto);
            await _db.SaveChangesAsync();
            return Ok(producto);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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