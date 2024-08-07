using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApp.Data;
using tourismApp.DTOs.LoginDto;

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
        public async Task<IActionResult> Login ([FromBody] LoginDto loginDto) {

            var registered = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
    
   
            if (registered == null)
                 {
                return BadRequest("User not found.");
                }
            
            if (registered.Password != loginDto.Password){

                return BadRequest("Passwowrd did not match ");
            }



            return StatusCode(200," Logged in");

        }


        
    }
}