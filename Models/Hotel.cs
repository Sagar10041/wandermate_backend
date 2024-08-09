using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tourismApp.Models
{
         [Table("Hotel")]
    public class Hotel
    {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    
        public int Id { get; set; }
        
        public string Name { get; set; } = String.Empty;

        public decimal Price { get; set; }

        public List<string> Image { get; set; } = new List<string>();

        public string Description { get; set; } = String.Empty;

        public int Rating {get; set;}

        public bool FreeCancellation {get; set;}
    
        public bool ReserveNow {get; set;}


        public List<HotelReviews> HotelReviews {get; set;} =new List<HotelReviews>();

         public List<HotelBooking> HotelBookings {get; set;} =new List<HotelBooking>();



    }
}
