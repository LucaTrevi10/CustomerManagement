using LoginService.cs.Data;
using LoginService.cs.Models;
using LoginService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LoginService.cs.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AuthDbContext _context;

        public AuthController(IConfiguration configuration, AuthDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("login/{username}/{password}")]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Cerca l'utente nel database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return Unauthorized(new { Message = "Username o password errati" });
            }

            // Hashiamo la password ricevuta per confrontarla con quella memorizzata
            string hashedInputPassword = HashPassword(password);

            // Confrontiamo gli hash
            if (user.PasswordHash != hashedInputPassword)
            {
                return Unauthorized(new { Message = "Username o password errati" });
            }

            // Genera il token JWT
            var token = GenerateJwtToken(user.Username);

            return Ok(new { Token = token });
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest(new { message = "Username già in uso" });
            }

            var user = new User
            {
                Username = request.Username,
                PasswordHash = HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registrazione completata!" });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private string GenerateJwtToken(string username)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiryMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        
    }
}
