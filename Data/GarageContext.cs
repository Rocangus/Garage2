using Garage2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            ParkedVehicle pay276 = new ParkedVehicle { RegistrationNumber = "PAY276", Colour = "Red", Manufacturer = "Skoda", Model = "Fabia Combi 1.2 TSI", NumberOfWheels = 4, Type = VehicleType.Car };
            modelBuilder.Entity<ParkedVehicle>()
                .HasData(
                    pay276
                );
            modelBuilder.Entity<ParkingContract>()
                .HasData(
                    new { Id=1, ParkingDate=DateTime.Parse("2019-12-13 07:35:26"), VehicleRegistrationNumber="PAY276"}
                );
        }

        public DbSet<ParkedVehicle> ParkedVehicles { get; set; }
        public DbSet<ParkingContract> Contracts { get; set; }
    }
}
