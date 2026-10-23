
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthController(
            RaceDayContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("All required fields must be provided.");
            }

            if (request.Password.Length < 8)
            {
                return BadRequest("Password must be at least 8 characters.");
            }

            string email = request.Email.Trim();

            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                return Conflict("An account with this email already exists.");
            }

            var user = new User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = email,
                PhoneNumber = request.PhoneNumber,
                Role = "Participant",
                CreatedAt = DateTime.Now
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email.Trim());

            if (user == null ||
                string.IsNullOrEmpty(user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            var verification = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, request.Password);

            if (verification == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid email or password.");
            }

            string? secret = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(secret) ||
                Encoding.UTF8.GetByteCount(secret) < 32)
            {
                return StatusCode(500,
                    "JWT signing key is missing or too short.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,
                    user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return Ok(new
            {
                message = "Login successful.",
                token = new JwtSecurityTokenHandler()
                    .WriteToken(token),
                expiresAt = token.ValidTo,
                user = new
                {
                    user.UserID,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Role
                }
            });
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new
            {
                message = "Logout successful. Discard your access token."
            });
        }
    }

    public class RegisterRequest
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string? PhoneNumber { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
