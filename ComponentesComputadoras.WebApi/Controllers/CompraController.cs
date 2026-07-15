using AutoMapper;
using ComponentesComputadoras.Application;
using ComponentesComputadoras.Application.Dtos.Compra;
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
    public class ComprasController : ControllerBase
    {
        private readonly DbDataAccess _context;
        private readonly IMapper _mapper;

        public ComprasController(DbDataAccess context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [Authorize]
        public IActionResult All()
        {
            var compras = _context.Compras
                .Include(c => c.Detalles)
                .ToList();

            return Ok(_mapper.Map<IList<CompraResponseDto>>(compras));
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var compra = _context.Compras
                .Include(c => c.Detalles)
                .FirstOrDefault(c => c.Id == id);

            if (compra is null) return NotFound();

            return Ok(_mapper.Map<CompraResponseDto>(compra));
        }

        [HttpPost]
        public IActionResult Crear([FromBody] CompraRequestDto compraRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validar proveedor
            var proveedor = _context.Proveedores.Find(compraRequestDto.ProveedorId);
            if (proveedor is null)
                return NotFound(new { error = "El proveedor no existe" });

            // Validar productos
            foreach (var detalle in compraRequestDto.Detalles)
            {
                var producto = _context.Productos.Find(detalle.ProductoId);
                if (producto is null)
                    return NotFound(new { error = $"El producto con Id {detalle.ProductoId} no existe" });
            }

            var compra = _mapper.Map<Compra>(compraRequestDto);
            _context.Compras.Add(compra);
            _context.SaveChanges();

            var response = _mapper.Map<CompraResponseDto>(compra);
            return CreatedAtAction(nameof(GetById), new { id = compra.Id }, response);
        }

        [HttpDelete("{id}")]
        public IActionResult Borrar(int id)
        {
            var compra = _context.Compras.Find(id);
            if (compra is null) return NotFound();

            _context.Compras.Remove(compra);
            _context.SaveChanges();

            return NoContent();
        }
    }
}