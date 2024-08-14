using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tourismApp.Data;
using tourismApp.DTOs.CurrentUserBookings;
using tourismApp.DTOs.HotelBookingDTO;
using tourismApp.Extensions;
using tourismApp.Models;

namespace tourismApp.Controllers
{
    [Authorize]
    [Route("api/userbookings")]
    [ApiController]
    public class UserHotelBooking : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserHotelBooking(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = User.GetUserId();
            var bookings = await _context.HotelBookings
                .Where(hb => hb.UserId == user)
                .Include(hb => hb.Hotel)
                .Include(hb => hb.AppUser)
                .ToListAsync();

            var bookingDTOs = bookings.Select(booking => new UserBookingDTO
            {
                Id = booking.Id,
                HotelName = booking.Hotel.Name,
                UserName = booking.AppUser.UserName,
                BookingDate = booking.BookingDate,
                Duration = booking.Duration,
                Checkin = booking.Checkin,
                Checkout = booking.Checkout,
                TotalPrice = booking.TotalPrice
            }).ToList();

            return Ok(bookingDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO bookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = User.GetUserId();
            var hotel = await _context.Hotel.FindAsync(bookingDto.HotelId);

            if (hotel == null)
            {
                return BadRequest("Invalid HotelId.");
            }

            var booking = new HotelBooking
            {
                HotelId = bookingDto.HotelId,
                UserId = user, // Automatically set the current user's ID
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
                return CreatedAtAction(nameof(GetAll), new { id = booking.Id }, booking); // Use GetAll as there's no GetById
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateHotelBookingDto bookingDto)
        {
            var user = await _userManager.GetUserAsync(User);
            var bookingInDatabase = await _context.HotelBookings
                .Where(hb => hb.Id == id && hb.UserId == user.Id)
                .FirstOrDefaultAsync();

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
                if (!_context.HotelBookings.Any(hb => hb.Id == id && hb.UserId == user.Id))
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
            var user = await _userManager.GetUserAsync(User);
            var bookingToDelete = await _context.HotelBookings
                .Where(hb => hb.Id == id && hb.UserId == user.Id)
                .FirstOrDefaultAsync();

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
