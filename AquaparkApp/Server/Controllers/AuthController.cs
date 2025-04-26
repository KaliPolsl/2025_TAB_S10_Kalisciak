using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AquaparkApp.Server.Data;
using AquaparkApp.Shared;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations; // Dodano brakuj�c� przestrze� nazw

namespace AquaparkApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest("U�ytkownik z tym adresem email ju� istnieje.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash); // Hashowanie has�a
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Rejestracja zako�czona sukcesem.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
            {
                return Unauthorized("Nieprawid�owy email lub has�o.");
            }

            // Mo�esz tutaj doda� logik� generowania tokenu JWT
            return Ok("Logowanie zako�czone sukcesem.");
        }
    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}
