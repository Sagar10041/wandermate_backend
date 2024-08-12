// using System;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace tourismApp.Models
// {
//     public class HotelBooking
//     {
//         [Key]
//         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//         public int Id { get; set; }

//         public int HotelId { get; set; }
//         public Hotel Hotel { get; set; }

//         public int UserId { get; set; }
//         public Users User { get; set; }

//         public DateTime BookingDate { get; set; }

//         public int Duration { get; set; }

//         public DateTime Checkin { get; set; }

//         public DateTime Checkout { get; set; }

//         public decimal TotalPrice { get; set; }
//     }
// }
