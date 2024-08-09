using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApp.Data;
using tourismApp.DTOs.HotelBookingDTO;
using tourismApp.Models;

namespace tourismApp.Controllers
{
    [Authorize]
    [Route("api/bookings")]
    [ApiController]
    public class HotelBookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HotelBookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _context.HotelBookings
                .Include(hb => hb.Hotel)
                .Include(hb => hb.User)
                .ToListAsync();

            var bookingDTOs = bookings.Select(booking => new HotelBookingDto
            {
                Id = booking.Id,
                HotelName = booking.Hotel.Name,
                UserName = booking.User.Username,
                BookingDate = booking.BookingDate,
                Duration = booking.Duration,
                Checkin = booking.Checkin,
                Checkout = booking.Checkout,
                TotalPrice = booking.TotalPrice
            }).ToList();

            return Ok(bookingDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var booking = await _context.HotelBookings
                .Where(hb => hb.Id == id)
                .Include(hb => hb.Hotel)
                .Include(hb => hb.User)
                .Select(hb => new HotelBookingDto
                {
                    Id = hb.Id,
                    HotelName = hb.Hotel.Name,
                    UserName = hb.User.Username,
                    BookingDate = hb.BookingDate,
                    Duration = hb.Duration,
                    Checkin = hb.Checkin,
                    Checkout = hb.Checkout,
                    TotalPrice = hb.TotalPrice
                })
                .FirstOrDefaultAsync();

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateHotelBookingDto bookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hotel = await _context.Hotel.FindAsync(bookingDto.HotelId);
            var user = await _context.Users.FindAsync(bookingDto.UserId);

            if (hotel == null || user == null)
            {
                return BadRequest("Invalid HotelId or UserId.");
            }

            var booking = new HotelBooking
            {
                HotelId = bookingDto.HotelId,
                UserId = bookingDto.UserId,
                BookingDate = bookingDto.BookingDate,
                Duration = bookingDto.Duration,
                Checkin = bookingDto.Checkin,
                Checkout = bookingDto.Checkout,
                TotalPrice = bookingDto.TotalPrice
            };

            try
            {
                await _context.HotelBookings.AddAsync(booking);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = booking.Id }, bookingDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateHotelBookingDto bookingDto)
        {
            var bookingInDatabase = await _context.HotelBookings.FindAsync(id);
            if (bookingInDatabase == null)
            {
                return NotFound();
            }

            bookingInDatabase.BookingDate = bookingDto.BookingDate;
            bookingInDatabase.Duration = bookingDto.Duration;
            bookingInDatabase.Checkin = bookingDto.Checkin;
            bookingInDatabase.Checkout = bookingDto.Checkout;
            bookingInDatabase.TotalPrice = bookingDto.TotalPrice;

            _context.Entry(bookingInDatabase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.HotelBookings.Any(hb => hb.Id == id))
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

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking([FromRoute] int id)
        {
            var bookingToDelete = await _context.HotelBookings.FindAsync(id);

            if (bookingToDelete == null)
            {
                return NotFound();
            }

            try
            {
                _context.HotelBookings.Remove(bookingToDelete);
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
