using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tourismApp.DTOs.CurrentUserBookings
{
    public class CreateBookingDTO
    {
         [Required]
        public int HotelId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public DateTime Checkin { get; set; }

        [Required]
        public DateTime Checkout { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalPrice { get; set; }
    }
}