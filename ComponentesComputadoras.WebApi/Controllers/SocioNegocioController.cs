using AutoMapper;
using ComponentesComputadoras.Application;
using ComponentesComputadoras.Application.Dtos.SocioNegocio;
using ComponentesComputadoras.Datos;
using ComponentesComputadoras.Entities;
using ComponentesComputadoras.Entities.MicrosoftIdentity;
using ComponentesComputadoras.Enumeraciones;
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
    public class SocioNegocioController : ControllerBase
    {
        private readonly DbDataAccess _context;
        private readonly IMapper _mapper;

        public SocioNegocioController(DbDataAccess context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       
        [HttpGet("All")]
        [Authorize]
        public IActionResult All()
        {
            var socios = _context.SociosNegocio
                .Include(sn => sn.Cliente)
                .Include(sn => sn.Proveedor)
                .ToList();

            var dto = _mapper.Map<IList<SocioNegocioResponseDto>>(socios);
            return Ok(dto);
        }

        //  Obtener socio por Id
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var socio = _context.SociosNegocio
                .Include(sn => sn.Cliente)
                .Include(sn => sn.Proveedor)
                .FirstOrDefault(sn => sn.Id == id);

            if (socio is null) return NotFound();

            var dto = _mapper.Map<SocioNegocioResponseDto>(socio);
            return Ok(dto);
        }

        //  Crear socio (usuarios autenticados)
        [HttpPost]
        [Authorize]
        public IActionResult Crear([FromBody] SocioNegocioRequestDto socioRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (socioRequest.Tipo == TipoSocio.Cliente && socioRequest.ClienteId is null)
                return BadRequest(new { error = "Debe especificar ClienteId para tipo Cliente." });
            if (socioRequest.Tipo == TipoSocio.Proveedor && socioRequest.ProveedorId is null)
                return BadRequest(new { error = "Debe especificar ProveedorId para tipo Proveedor." });

            var socio = _mapper.Map<SocioNegocio>(socioRequest);
            _context.SociosNegocio.Add(socio);
            _context.SaveChanges();

            var socioConRelaciones = _context.SociosNegocio
                .Include(sn => sn.Cliente)
                .Include(sn => sn.Proveedor)
                .FirstOrDefault(sn => sn.Id == socio.Id);

            var dto = _mapper.Map<SocioNegocioResponseDto>(socioConRelaciones);
            return CreatedAtAction(nameof(GetById), new { id = socio.Id }, dto);
        }

      
      

       
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Borrar(int id)
        {
            var socio = _context.SociosNegocio.Find(id);
            if (socio is null) return NotFound();

            _context.SociosNegocio.Remove(socio);
            _context.SaveChanges();

            return NoContent();
        }
    }
}