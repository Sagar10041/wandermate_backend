using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApp.Data;
using tourismApp.DTOs.UserDTO;
using tourismApp.Migrations;
using tourismApp.Models;

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

            var users = userlist.Select( user => new UserDto {

                Id = user.Id,
                Username = user.Username,
                Email= user.Email
               
                
                
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
                    Email= user.Email
                   
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
            var olduser = await _context.Users.ToListAsync();

            foreach (var user in olduser){

                if (user.Username == userDto.Username ) {

                    return BadRequest();
                }
            }

            var newUser = new Users
            {
                Username = userDto.Username,
                Email= userDto.Email,
                Password =userDto.Password
            };

            try
            {
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, userDto);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {

           var registeredUser = await _context.Users.FindAsync(id);
            if (registeredUser  == null)
            {
                return NotFound();
            }

                registeredUser.Username = userDto.Username;
                registeredUser.Email= userDto.Email;
                registeredUser.Password =userDto.Password;

            _context.Entry(registeredUser ).State = EntityState.Modified;

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
              
                return StatusCode(500, ex);
            }

            return StatusCode(200," User updated " );
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
               
                return StatusCode(500, ex);
            }

            return NoContent();
        }

}
}