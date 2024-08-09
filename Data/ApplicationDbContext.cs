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

        public DbSet<HotelBooking> HotelBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many 
            modelBuilder.Entity<HotelBooking>()
                .HasOne(hb => hb.Hotel)
                .WithMany(h => h.HotelBookings)
                .HasForeignKey(hb => hb.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HotelBooking>()
                .HasOne(hb => hb.User)
                .WithMany(u => u.HotelBookings)
                .HasForeignKey(hb => hb.UserId)
                .OnDelete(DeleteBehavior.Restrict);

             modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();

            

            
        }

    }
}