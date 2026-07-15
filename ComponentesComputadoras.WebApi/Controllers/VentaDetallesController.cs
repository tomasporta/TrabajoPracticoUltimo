namespace ComponentesComputadoras.WebApi.Controllers
{
    using AutoMapper;
    using global::ComponentesComputadoras.Application;
    using global::ComponentesComputadoras.Application.Dtos.VentaDetalle;
    using global::ComponentesComputadoras.Datos;
    using global::ComponentesComputadoras.Entities;
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
        public class VentaDetallesController : ControllerBase
        {
            private readonly DbDataAccess _context;
            private readonly IMapper _mapper;

            public VentaDetallesController(DbDataAccess context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            [HttpGet("All")]
            [Authorize]
            public IActionResult All()
            {
                var detalles = _context.VentaDetalles
                    .Include(vd => vd.Producto)
                    .Include(vd => vd.Venta)
                    .ToList();

                return Ok(_mapper.Map<IList<VentaDetalleResponseDto>>(detalles));
            }

            [HttpGet("{id}")]
            public IActionResult GetById(int id)
            {
                var detalle = _context.VentaDetalles
                    .Include(vd => vd.Producto)
                    .Include(vd => vd.Venta)
                    .FirstOrDefault(vd => vd.Id == id);

                if (detalle is null) return NotFound();

                return Ok(_mapper.Map<VentaDetalleResponseDto>(detalle));
            }

            [HttpPost]
            public IActionResult Crear([FromBody] VentaDetalleRequestDto detalleRequest)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // Validar existencia de Venta y Producto
                var venta = _context.Ventas.Find(detalleRequest.VentaId);
                var producto = _context.Productos.Find(detalleRequest.ProductoId);
                if (venta is null || producto is null)
                    return NotFound(new { error = "Venta o Producto no existe" });

                var detalle = _mapper.Map<VentaDetalle>(detalleRequest);
                _context.VentaDetalles.Add(detalle);
                _context.SaveChanges();

                var response = _mapper.Map<VentaDetalleResponseDto>(detalle);
                return CreatedAtAction(nameof(GetById), new { id = detalle.Id }, response);
            }




            [HttpDelete("{id}")]
            public IActionResult Borrar(int id)
            {
                var detalle = _context.VentaDetalles.Find(id);
                if (detalle is null) return NotFound();

                _context.VentaDetalles.Remove(detalle);
                _context.SaveChanges();

                return NoContent();
            }
        }

    }
}
