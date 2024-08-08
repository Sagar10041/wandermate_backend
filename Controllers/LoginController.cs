using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApp.Data;
using tourismApp.DTOs.LoginDto;
using BCrypt.Net;

namespace tourismApp.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var registeredUser = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == loginDto.Username);

            if (registeredUser == null)
            {
                return BadRequest("User not found.");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, registeredUser.Password);

            if (!isPasswordValid)
            {
                return BadRequest("Password did not match.");
            }

            // Create claims and identity
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, registeredUser.Username),
                new Claim(ClaimTypes.Email, registeredUser.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true // Set to true if you want to persist the authentication across browser sessions
                });

            return Ok("Logged in");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out");
        }
    }
}
