using AutoMapper;
using ComponentesComputadoras.Application;
using ComponentesComputadoras.Application.Dtos.TipoProducto;
using ComponentesComputadoras.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComponentesComputadoras.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoProductosController : ControllerBase
    {
        private readonly ILogger<TipoProductosController> _logger;
        private readonly IApplication<TipoProducto> _tipoProducto;
        private readonly IMapper _mapper;

        public TipoProductosController(
            ILogger<TipoProductosController> logger,
            IApplication<TipoProducto> tipoProducto,
            IMapper mapper)
        {
            _logger = logger;
            _tipoProducto = tipoProducto;
            _mapper = mapper;
        }

        //  Obtener todos los tipos de producto
        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            var tipos = _tipoProducto.GetAll();
            var dto = _mapper.Map<IList<TipoProductoResponseDto>>(tipos);
            return Ok(dto);
        }

        //  Obtener tipo de producto por Id
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var tipo = _tipoProducto.GetById(id);
            if (tipo is null) return NotFound();

            var dto = _mapper.Map<TipoProductoResponseDto>(tipo);
            return Ok(dto);
        }

        //  Crea tipo de producto
        [HttpPost]
        [Authorize]
        public IActionResult Crear([FromBody] TipoProductoRequestDto tipoProductoRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tipo = _mapper.Map<TipoProducto>(tipoProductoRequestDto);
            _tipoProducto.Save(tipo);

            var dto = _mapper.Map<TipoProductoResponseDto>(tipo);
            return CreatedAtAction(nameof(GetById), new { id = tipo.Id }, dto);
        }


        //  Borrar tipo de producto
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Borrar(int id)
        {
            var tipoBack = _tipoProducto.GetById(id);
            if (tipoBack is null) return NotFound();

            _tipoProducto.Delete(tipoBack.Id);
            return NoContent();
        }
    }

}
