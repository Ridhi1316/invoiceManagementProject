using Microsoft.AspNetCore.Mvc;
using InvoiceManagement1.Auth;
using Microsoft.Extensions.Configuration;
using InvoiceManagement1.DTOs;


namespace InvoiceManagement1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Login and get JWT token.
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            // Hardcoded check or use IUserService if implemented
            if (loginDto.Username == "admin" && loginDto.Password == "admin123")
            {
                // Generate token using static method
                var token = TokenService.GenerateToken(loginDto.Username, _configuration["JwtSettings:SecretKey"]);
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
