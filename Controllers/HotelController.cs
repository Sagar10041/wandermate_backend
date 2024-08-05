using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApp.Data;
using tourismApp.Models;

namespace tourismApp.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var hotels = await _context.Hotel.ToListAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        {
            var hotel = await _context.Hotel.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] Hotel hotel) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _context.Hotel.AddAsync(hotel);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotel);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] Hotel hotel)
        {

            var hotelInDatabase = await _context.Hotel.FindAsync(id);
            if (hotelInDatabase == null)
            {
                return NotFound();
            }

            hotelInDatabase.Name = hotel.Name;
            hotelInDatabase.Price = hotel.Price;
            hotelInDatabase.Image = hotel.Image;
            hotelInDatabase.Description = hotel.Description;
            hotelInDatabase.Rating = hotel.Rating;
            hotelInDatabase.FreeCancellation = hotel.FreeCancellation;
            hotelInDatabase.ReserveNow = hotel.ReserveNow;

            _context.Entry(hotelInDatabase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Hotel.Any(h => h.Id == id))
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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel([FromRoute] int id) 
        {
            var hotelToDelete = await _context.Hotel.FindAsync(id);

            if (hotelToDelete == null)
            {
                return NotFound();
            }

            try
            {
                _context.Hotel.Remove(hotelToDelete);
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
