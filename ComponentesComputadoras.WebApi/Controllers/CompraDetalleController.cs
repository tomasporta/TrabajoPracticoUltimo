namespace ComponentesComputadoras.WebApi.Controllers
{
    using AutoMapper;
    using global::ComponentesComputadoras.Application;
    using global::ComponentesComputadoras.Application.Dtos.CompraDetalle;
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
        public class CompraDetallesController : ControllerBase
        {
            private readonly DbDataAccess _context;
            private readonly IMapper _mapper;

            public CompraDetallesController(DbDataAccess context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            [HttpGet("All")]
            [Authorize]
            public IActionResult All()
            {
                var detalles = _context.CompraDetalles
                    .Include(cd => cd.Producto)
                    .Include(cd => cd.Compra)
                    .ToList();

                return Ok(_mapper.Map<IList<CompraDetalleResponseDto>>(detalles));
            }

            [HttpGet("{id}")]
            public IActionResult GetById(int id)
            {
                var detalle = _context.CompraDetalles
                    .Include(cd => cd.Producto)
                    .Include(cd => cd.Compra)
                    .FirstOrDefault(cd => cd.Id == id);

                if (detalle is null) return NotFound();

                return Ok(_mapper.Map<CompraDetalleResponseDto>(detalle));
            }

            [HttpPost]
            public IActionResult Crear([FromBody] CompraDetalleRequestDto detalleRequest)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var compra = _context.Compras.Find(detalleRequest.CompraId);
                var producto = _context.Productos.Find(detalleRequest.ProductoId);
                if (compra is null || producto is null)
                    return NotFound(new { error = "Compra o Producto no existe" });

                var detalle = _mapper.Map<CompraDetalle>(detalleRequest);
                _context.CompraDetalles.Add(detalle);
                _context.SaveChanges();

                var response = _mapper.Map<CompraDetalleResponseDto>(detalle);
                return CreatedAtAction(nameof(GetById), new { id = detalle.Id }, response);
            }



            [HttpDelete("{id}")]
            public IActionResult Borrar(int id)
            {
                var detalle = _context.CompraDetalles.Find(id);
                if (detalle is null) return NotFound();

                _context.CompraDetalles.Remove(detalle);
                _context.SaveChanges();

                return NoContent();
            }
        }

    }
}