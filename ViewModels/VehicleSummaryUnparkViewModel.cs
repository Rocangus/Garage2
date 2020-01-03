using Garage2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.ViewModels
{
    public class VehicleSummaryUnparkViewModel
    {

        private ParkedVehicle models;

        public VehicleSummaryUnparkViewModel(ParkedVehicle models)
        {
            this.models = models;
        }

        public string RegistrationNumber { get; set; }

        public VehicleType Type { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

       
    }
}
