using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FYPManagementSystem.Data;
using FYPManagementSystem.Models;
using FYPManagementSystem.Models.DTOs;

namespace FYPManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config  = config;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return BadRequest(new { message = "Email already registered." });

            var user = new User
            {
                Name      = dto.Name,
                Email     = dto.Email,
                Password  = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role      = dto.Role,
                Expertise = dto.Expertise,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful!" });
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized(new { message = "Invalid email or password." });

            var token = GenerateJwtToken(user);

            return Ok(new AuthResponseDto
            {
                Token  = token,
                Name   = user.Name,
                Email  = user.Email,
                Role   = user.Role,
                UserId = user.Id
            });
        }

        // GET api/auth/users  (Admin only)
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new {
                    u.Id, u.Name, u.Email, u.Role, u.Expertise, u.CreatedAt
                })
                .ToListAsync();

            return Ok(users);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _config["Jwt:Key"] ?? "FYP_SuperSecret_Key_2024_Secure_12345678";
            var key    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds  = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role,               user.Role),
                new Claim("userId",                      user.Id.ToString()),
                new Claim("name",                        user.Name)
            };

            var token = new JwtSecurityToken(
                issuer:             _config["Jwt:Issuer"],
                audience:           _config["Jwt:Audience"],
                claims:             claims,
                expires:            DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
