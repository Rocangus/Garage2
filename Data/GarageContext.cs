﻿using Garage2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Data
{
    public class GarageContext : DbContext
    {
        public GarageContext(DbContextOptions<GarageContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Member member = new Member 
            { MemberId = 1, FirstName = "Henning",
              LastName = "Odén", Email = "henning.oden@outlook.com", 
              CityAddress = "Lindevägen 60 Enskede Gård", PhoneNumber = "0739753838" 
            };
            modelBuilder.Entity<Member>().HasData(
                member);
            modelBuilder.Entity<ParkedVehicle>().HasData(
                new ParkedVehicle
                {
                    RegistrationNumber = "PAY276",
                    Colour = "Red",
                    Manufacturer = "Skoda",
                    Model = "Fabia Combi 1.2 TSI",
                    MemberId = 1,
                    NumberOfWheels = 4,
                    ParkingDate = DateTime.Parse("2019-12-26T19:08:27", new CultureInfo("sv-SE")), 
                    Type=VehicleType.Car
                }); 
        }

        public DbSet<ParkedVehicle> ParkedVehicles { get; set; }
        public DbSet<Member> Members { get; set; }
    }
}
