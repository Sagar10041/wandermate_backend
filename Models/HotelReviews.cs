using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tourismApp.Models
{
    public class HotelReviews
    {
     [Key]
        public int ReviewId { get; set; }

        public int Rating { get; set; }

        public string ReviewText { get; set; } =String.Empty;

        public DateTime CreatedOn {get; set;} = DateTime.UtcNow;
        
        public int? HotelId {get; set;}
        public Hotel? Hotel {get; set;}
    }
}