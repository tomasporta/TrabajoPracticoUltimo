using AutoMapper;
using ComponentesComputadoras.Application;
using ComponentesComputadoras.Application.Dtos.Producto;
using ComponentesComputadoras.DataAccess;
using ComponentesComputadoras.Datos;
using ComponentesComputadoras.Entities;
using ComponentesComputadoras.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComponentesComputadoras.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ILogger<ProductosController> _logger;
        private readonly IApplication<Producto> _producto;
        private readonly IMapper _mapper;
        private readonly DbDataAccess _context;

        public ProductosController(DbDataAccess context, IMapper mapper, ILogger<ProductosController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("All")]
        [Authorize]
        public IActionResult All()
        {
            var productos = _context.Productos
    .Include(p => p.TipoProducto)
    .Include(p => p.Proveedor)
    .ToList();
            var dto = _mapper.Map<IList<ProductoResponseDto>>(productos);
            return Ok(dto);

        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var producto = _producto.GetById(id);
            if (producto is null) return NotFound();

            var dto = _mapper.Map<ProductoResponseDto>(producto);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Crear([FromBody] ProductoRequestDto productoRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var producto = _mapper.Map<Producto>(productoRequestDto);
            _context.Productos.Add(producto);
            _context.SaveChanges();

            //  Reconsultar con relaciones
            var productoConRelaciones = _context.Productos
                .Include(p => p.TipoProducto)
                .Include(p => p.Proveedor)
                .FirstOrDefault(p => p.Id == producto.Id);

            var dto = _mapper.Map<ProductoResponseDto>(productoConRelaciones);
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, dto);
        }





        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Borrar(int id)
        {
            var productoBack = _producto.GetById(id);
            if (productoBack is null) return NotFound();

            _producto.Delete(productoBack.Id);
            return NoContent();
        }
    }

}
