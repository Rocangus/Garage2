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
            ParkedVehicle aaa123 = new ParkedVehicle { RegistrationNumber = "AAA123", Colour = "White", Manufacturer = "MAN", Model = "Buss", NumberOfWheels = 6, Type = VehicleType.Bus };
            ParkedVehicle huj63e = new ParkedVehicle { RegistrationNumber = "HUJ63E", Colour = "Blue", Manufacturer = "BMW", Model = "S1000RR", NumberOfWheels = 2, Type = VehicleType.Motorcycle };
            modelBuilder.Entity<ParkedVehicle>()
                .HasData(
                    pay276,
                    aaa123,
                    huj63e
                );
        }

        public DbSet<ParkedVehicle> ParkedVehicles { get; set; }
        public DbSet<ParkingContract> Contracts { get; set; }
    }
}
