using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using tourismApp.Models;

namespace tourismApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
         public ApplicationDbContext(DbContextOptions dbContextOptions) 
        : base (dbContextOptions)
        {    
        }

        public DbSet <Hotel> Hotel {get; set;}

        public DbSet <HotelReviews> HotelReviews {get; set;}

        // public DbSet<HotelBooking> HotelBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // // Configure many-to-many 
            // builder.Entity<HotelBooking>()
            //     .HasOne(hb => hb.Hotel)
            //     .WithMany(h => h.HotelBookings)
            //     .HasForeignKey(hb => hb.HotelId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // builder.Entity<HotelBooking>()
            //     .HasOne(hb => hb.User)
            //     .WithMany(u => u.HotelBookings)
            //     .HasForeignKey(hb => hb.UserId)
            //     .OnDelete(DeleteBehavior.Restrict);

            // builder.Entity<Users>()
            //     .HasIndex(u => u.Email)
            //     .IsUnique();


             List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole{
                    Name ="Admin",
                    NormalizedName="ADMIN"
                },
                new IdentityRole{
                    Name ="User",
                    NormalizedName="USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);

            

            
        }

    }
}