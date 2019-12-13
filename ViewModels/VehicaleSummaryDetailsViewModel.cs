using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garage2.Models;

namespace Garage2.ViewModels
{
    public class VehicaleSummaryDetailsViewModel
    {
        private ParkedVehicle models;

        public VehicaleSummaryDetailsViewModel(ParkedVehicle models)
        {
            this.models = models;
        }

        public string RegistrationNumber { get; set; }

        public VehicleType Type { get; set; }

        public string Colour { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public int NumberOfWheels { get; set; }

        public TimeSpan ParkingTime { get; set; }

    }
}
