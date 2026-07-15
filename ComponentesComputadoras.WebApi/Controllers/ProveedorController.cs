using AutoMapper;
using ComponentesComputadoras.Application;
using ComponentesComputadoras.Application.Dtos.Proveedor;
using ComponentesComputadoras.Entities;
using ComponentesComputadoras.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ComponentesComputadoras.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly ILogger<ProveedoresController> _logger;
        private readonly IApplication<Proveedor> _proveedor;
        private readonly IMapper _mapper;

        public ProveedoresController(
            ILogger<ProveedoresController> logger,
            IApplication<Proveedor> proveedor,
            IMapper mapper)
        {
            _logger = logger;
            _proveedor = proveedor;
            _mapper = mapper;
        }

        
        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            var proveedores = _proveedor.GetAll();
            var dto = _mapper.Map<IList<ProveedorResponseDto>>(proveedores);
            return Ok(dto);
        }

      
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var proveedor = _proveedor.GetById(id);
            if (proveedor is null) return NotFound();

            var dto = _mapper.Map<ProveedorResponseDto>(proveedor);
            return Ok(dto);
        }

     
        [HttpPost]
        [Authorize]
        public IActionResult Crear([FromBody] ProveedorRequestDto proveedorRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var proveedor = _mapper.Map<Proveedor>(proveedorRequestDto);
            _proveedor.Save(proveedor);

            var dto = _mapper.Map<ProveedorResponseDto>(proveedor);
            return CreatedAtAction(nameof(GetById), new { id = proveedor.Id }, dto);
        }


       



        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Borrar(int id)
        {
            var proveedorBack = _proveedor.GetById(id);
            if (proveedorBack is null) return NotFound();

            _proveedor.Delete(proveedorBack.Id);
            return NoContent();
        }
    }
}