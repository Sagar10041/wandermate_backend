using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tourismApp.Models;

namespace tourismApp.DTOs.HotelDTO
{
    public class HotelDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = String.Empty;

        public decimal Price { get; set; }

        public List<string> Image { get; set; } = new List<string>();

        public string Description { get; set; } = String.Empty;

        public int Rating {get; set;}

        public bool FreeCancellation {get; set;}
    
        public bool ReserveNow {get; set;}

    }
}