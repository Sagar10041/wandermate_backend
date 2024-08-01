using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tourismApp.Data;
using tourismApp.Models;

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

        [HttpPost]

        public IActionResult CreateHotel ([FromBody] Hotel hotel) {

            if (hotel == null ) {

                return BadRequest();

            }
            _context.Hotel.Add(hotel);
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(GetById), new {id = hotel.Id},hotel);
        }

        [HttpPut("{id}")]

        public IActionResult UpdateHotel (int id ,[FromBody] Hotel hotel){

            var hotelInDatabase = _context.Hotel.Find(id);
            if (hotelInDatabase == null){
                return NotFound();
            }

            hotelInDatabase.Name = hotel.Name;
            hotelInDatabase.Name = hotel.Name;
            hotelInDatabase.Price = hotel.Price;
            hotelInDatabase.Image = hotel.Image;
            hotelInDatabase.Description = hotel.Description;
            hotelInDatabase.Rating = hotel.Rating;
            hotelInDatabase.FreeCancellation = hotel.FreeCancellation;
            hotelInDatabase.ReserveNow = hotel.ReserveNow;

            _context.Hotel.Update(hotelInDatabase);
            _context.SaveChanges();


            return NoContent();
        }

        [HttpDelete("{id}")]

        public IActionResult DeleteHotel ([FromRoute] int id) {

            var hotelToDelete = _context.Hotel.Find(id);

            if (hotelToDelete == null){
                return NotFound();
            }

            _context.Hotel.Remove(hotelToDelete);
            _context.SaveChanges();

            return NoContent();
        }
    }
}