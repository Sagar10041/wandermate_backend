using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApp.Data;
using tourismApp.DTOs.UserDTO;
using tourismApp.Models;
using BCrypt.Net; // Import the BCrypt namespace

namespace tourismApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SignUpController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var userlist = await _context.Users.ToListAsync();

            var users = userlist.Select(user => new UserDto 
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            }).ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        {
            var user = await _context.Users.Where(u => u.Id == id)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto) 
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Username == userDto.Username);
            if (existingUser)
            {
                return BadRequest("Username already exists.");
            }

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var newUser = new Users
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = hashedPassword
            };

            try
            {
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            var registeredUser = await _context.Users.FindAsync(id);
            if (registeredUser == null)
            {
                return NotFound();
            }

            registeredUser.Username = userDto.Username;
            registeredUser.Email = userDto.Email;
            // Hash the password
            
            registeredUser.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            

            _context.Entry(registeredUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok("User updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id) 
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return NoContent();
        }
    }
}
