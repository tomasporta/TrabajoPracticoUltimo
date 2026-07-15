using AutoMapper;
using ComponentesComputadoras.Application.Dtos.Venta;
using ComponentesComputadoras.Datos;
using ComponentesComputadoras.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComponentesComputadoras.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly DbDataAccess _context;
        private readonly IMapper _mapper;

        public VentasController(DbDataAccess context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("All")]
        [Authorize]
        public IActionResult All()
        {
            var ventas = _context.Ventas
                .Include(v => v.Detalles)
                .ToList();

            return Ok(_mapper.Map<IList<VentaResponseDto>>(ventas));
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var venta = _context.Ventas
                .Include(v => v.Detalles)
                .FirstOrDefault(v => v.Id == id);

            if (venta is null) return NotFound();

            return Ok(_mapper.Map<VentaResponseDto>(venta));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Crear([FromBody] VentaRequestDto ventaRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var venta = _mapper.Map<Venta>(ventaRequestDto);
            _context.Ventas.Add(venta);
            _context.SaveChanges();

            var ventaConRelaciones = _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefault(v => v.Id == venta.Id);

            var dto = _mapper.Map<VentaResponseDto>(ventaConRelaciones);
            return CreatedAtAction(nameof(GetById), new { id = venta.Id }, dto);
        }


        [HttpDelete("{id}")]
        public IActionResult Borrar(int id)
        {
            var venta = _context.Ventas.Find(id);
            if (venta is null) return NotFound();

            _context.Ventas.Remove(venta);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
