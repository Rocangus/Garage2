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

        public DbSet<ParkedVehicle> ParkedVehicles { get; set; }
        public DbSet<ParkingContract> Contracts { get; set; }
    }
}
