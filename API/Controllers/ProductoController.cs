using System.Net;
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
    public class ProductoController : ControllerBase
    {
        private ResponseDto _response;
        private readonly ILogger<ProductoController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnidadTrabajo _unidadTrabajo;
        public ProductoController(IUnidadTrabajo unidadTrabajo, ILogger<ProductoController> logger, IMapper mapper)
        {
            _unidadTrabajo = unidadTrabajo;           
            _mapper = mapper;
            _logger = logger;
            _response = new ResponseDto(); 

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
             _logger.LogInformation("Listado de Productos");
            var listas = await _unidadTrabajo.Producto.ObtenerTodos();
            _response.Resultado =  listas;
            _response.Mensaje = "Lista de productos";
            _response.StatusCode = HttpStatusCode.OK;
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
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);                
            }

            var Prod = await _unidadTrabajo.Producto.ObtenerPrimero(c=>c.Id==id);
            if (Prod == null)
            {
                _logger.LogError("Producto No Existe!");
                _response.Mensaje ="Producto no existe!";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);   
                
            }
             _response.Resultado =  Prod;
            _response.Mensaje = "Datos del producto" + Prod.Id;
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Producto>>> PostProducto([FromBody] ProductoDto productoDto)
        {
            if (productoDto == null)
            {
                _response.Mensaje =" Informacion Incorrecta del producto";
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);   
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ProdExiste = await _unidadTrabajo.Producto.ObtenerPrimero
                                                ( p => p.Nombre.ToLower() == productoDto.Nombre.ToLower());
            if (ProdExiste != null)
            {
                // ModelState.AddModelError("NombreDuplicado", "Nombre de producto ya existe"); // model state personaliazado
                _response.IsExitoso = false;
                _response.Mensaje = "Nombre del Producto ya existe";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            Producto producto = _mapper.Map<Producto>(productoDto);

            await _unidadTrabajo.Producto.Agregar(producto);
            await _unidadTrabajo.Guardar();
             _response.IsExitoso = false;
            _response.Mensaje = "Producto Guardado con exito";
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetProducto", new {id= producto.Id}, producto); //status 201
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Producto>>> PutProducto(int id, [FromBody] ProductoDto productoDto)
        {
            if(id != productoDto.Id){
                _response.IsExitoso = false;
                _response.Mensaje = "Id de Producto no  coincide";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
               
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ProdExiste = await _unidadTrabajo.Producto.ObtenerPrimero
                                                ( p => p.Nombre.ToLower() == productoDto.Nombre.ToLower()
                                                && p.Id!= productoDto.Id);
            if (ProdExiste != null)
            {
                // ModelState.AddModelError("NombreDuplicado", "Nombre de producto ya existe"); // model state personaliazado
                _response.IsExitoso = false;
                _response.Mensaje = "Nombre del producto ya existe";
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            Producto producto = _mapper.Map<Producto>(productoDto);
            _unidadTrabajo.Producto.Actualizar(producto);
            await _unidadTrabajo.Guardar();
           _response.IsExitoso = false;
            _response.Mensaje = "Producto Actualizado";
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProducto(int id)
        {
            var producto = await _unidadTrabajo.Producto.ObtenerPrimero(c=>c.Id==id);
            if(producto == null){
                _response.IsExitoso = false;
                _response.Mensaje = "Producto No existe";
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _unidadTrabajo.Producto.Remover(producto);
            await _unidadTrabajo.Guardar();
            _response.IsExitoso = false;
            _response.Mensaje = "Producto Eliminado";
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }


    }
}