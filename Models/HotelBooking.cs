using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tourismApp.Models
{
    public class HotelBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        public DateTime BookingDate { get; set; }

        public int Duration { get; set; }

        public DateTime Checkin { get; set; }

        public DateTime Checkout { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
