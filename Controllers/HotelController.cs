using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApp.Data;
using tourismApp.DTOs.HotelDTO;
using tourismApp.Models;

namespace tourismApp.Controllers
{   [Authorize]
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

            var hotelDTO = hotels.Select( hotel => new HotelDto {

                Id = hotel.Id,
                Name = hotel.Name,
                Price=hotel.Price,
                Description=hotel.Description,
                Rating =hotel.Rating,
                FreeCancellation =hotel.FreeCancellation,
                ReserveNow = hotel.ReserveNow,
                
            }).ToList();
            return Ok(hotelDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id) 
        {
             var hotel = await _context.Hotel.Where(h => h.Id == id)
             .Select(h => new HotelDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Price = h.Price,
                    Image = h.Image,
                    Description = h.Description,
                    Rating = h.Rating,
                    FreeCancellation = h.FreeCancellation,
                    ReserveNow = h.ReserveNow
                })
                .FirstOrDefaultAsync();

             if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto hotelDto) 
        {
           if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hotel = new Hotel
            {
                Name = hotelDto.Name,
                Price = hotelDto.Price,
                Image = hotelDto.Image,
                Description = hotelDto.Description,
                Rating = hotelDto.Rating,
                FreeCancellation = hotelDto.FreeCancellation,
                ReserveNow = hotelDto.ReserveNow
            };

            try
            {
                await _context.Hotel.AddAsync(hotel);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotelDto);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDto hotelDto)
        {

           var hotelInDatabase = await _context.Hotel.FindAsync(id);
            if (hotelInDatabase == null)
            {
                return NotFound();
            }

            hotelInDatabase.Name = hotelDto.Name;
            hotelInDatabase.Price = hotelDto.Price;
            hotelInDatabase.Image = hotelDto.Image;
            hotelInDatabase.Description = hotelDto.Description;
            hotelInDatabase.Rating = hotelDto.Rating;
            hotelInDatabase.FreeCancellation = hotelDto.FreeCancellation;
            hotelInDatabase.ReserveNow = hotelDto.ReserveNow;

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
