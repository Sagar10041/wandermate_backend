using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tourismApp.Models
{
    public class Users
    {
        [Key]
        public int Id {get; set;}

        [Required]
        public string Username {get; set;} = String.Empty;

        [Required]
        public string Email {get; set;} = String.Empty;

        [Required]
        public string Password {get; set;} = String.Empty;


         public List<HotelBooking> HotelBookings {get; set;} =new List<HotelBooking>();



    }
}