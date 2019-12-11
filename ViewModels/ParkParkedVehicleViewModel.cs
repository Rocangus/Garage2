using Garage2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.ViewModels
{
    public class ParkParkedVehicleViewModel
    {
        [RegularExpression(@"^([A-Z]|[a-z]){3}\d{2,3}([A-Z]|[a-z]){0,1}")]
        public string RegistrationNumber { get; set; }

        [Required]
        public VehicleType Type { get; set; }

        [MaxLength(15)]
        public string Colour { get; set; }

        [MaxLength(30)]
        public string Manufacturer { get; set; }

        [MaxLength(60)]
        public string Model { get; set; }

        [Range(2, 18)]
        public int NumberOfWheels { get; set; }

    }
}
