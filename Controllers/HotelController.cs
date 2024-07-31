using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tourismApp.Data;

namespace tourismApp.Controllers
{
    [Route("tourismApp/hotel")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() {
            var hotels = _context.Hotel.ToList();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id) {
            var hotel = _context.Hotel.Find(id);

            if (hotel == null){

                return NotFound();
            }
            return Ok(hotel);
        }
    }
}