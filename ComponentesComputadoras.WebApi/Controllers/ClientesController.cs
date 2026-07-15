using AutoMapper;
using ComponentesComputadoras.Application;
using ComponentesComputadoras.Application.Dtos.Cliente;
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
    public class ClientesController : ControllerBase
    {
        private readonly ILogger<ClientesController> _logger;
        private readonly IApplication<Cliente> _cliente;
        private readonly IMapper _mapper;

        public ClientesController(
            ILogger<ClientesController> logger,
            IApplication<Cliente> cliente,
            IMapper mapper)
        {
            _logger = logger;
            _cliente = cliente;
            _mapper = mapper;
        }

        
        [HttpGet("All")]
        [Authorize]
        public IActionResult All()
        {
            var clientes = _cliente.GetAll();
            var dto = _mapper.Map<IList<ClienteResponseDto>>(clientes);
            return Ok(dto);
        }

        
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var cliente = _cliente.GetById(id);
            if (cliente is null) return NotFound();

            var dto = _mapper.Map<ClienteResponseDto>(cliente);
            return Ok(dto);
        }

        
        [HttpPost]
        [Authorize]
        public IActionResult Crear([FromBody] ClienteRequestDto clienteRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var cliente = _mapper.Map<Cliente>(clienteRequestDto);
            _cliente.Save(cliente);

            var dto = _mapper.Map<ClienteResponseDto>(cliente);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, dto);
        }

        [HttpPut("{id}")]
        [Authorize]
      

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Borrar(int id)
        {
            var clienteBack = _cliente.GetById(id);
            if (clienteBack is null) return NotFound();

            _cliente.Delete(clienteBack.Id);
            return NoContent();
        }
    }
    }
