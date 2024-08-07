using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tourismApp.Models;

namespace tourismApp.Data
{
    public class ApplicationDbContext : DbContext
    {
         public ApplicationDbContext(DbContextOptions dbContextOptions) 
        : base (dbContextOptions)
        {    
        }

        public DbSet <Hotel> Hotel {get; set;}

        public DbSet <HotelReviews> HotelReviews {get; set;}

        public DbSet <Users> Users {get; set;}

    }
}