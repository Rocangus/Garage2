using Garage2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.ViewModels
{
    public class ParkedVehicleViewModel
    {
            public string RegistrationNumber { get; set; }

            public VehicleType Type { get; set; }

            public string Colour { get; set; }

            public string Manufacturer { get; set; }

            public string Model { get; set; }

            public int NumberOfWheels { get; set; }

            public DateTime ParkingDate { get; set; }
    }
}
