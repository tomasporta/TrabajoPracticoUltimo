using ComponentesComputadoras.Abstraccioness;
using ComponentesComputadoras.Entities;
using ComponentesComputadoras.Entities.MicrosoftIdentity;
using ComponentesComputadoras.Servicios.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComponentesComputadoras.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenHandlerService _tokenHandler;

        public AuthController(UserManager<User> userManager, ITokenHandlerService tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        // Endpoint de registro

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Usuario creado correctamente");
        }


        // Endpoint de login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                // Obtener roles del usuario
                var roles = await _userManager.GetRolesAsync(user);

                var parametros = new TokensParameters
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    Roles = roles
                };

                var token = _tokenHandler.GenerateJwtTokens(parametros);
                return Ok(new { Token = token });
            }
            return Unauthorized("Credenciales inválidas");
        }
    }

    
  
    
        public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }
    }

}

    public class LoginRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

