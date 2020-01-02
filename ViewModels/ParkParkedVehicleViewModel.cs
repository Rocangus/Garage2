using Garage2.Models;
using Microsoft.AspNetCore.Mvc;
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
        [Remote("Vehicles/ValidateRegistrationNumber")]
        [Display(Name ="Registration Number")]
        public string RegistrationNumber { get; set; }

        [Required]
        public int VehicleTypeId { get; set; }

        [MaxLength(15)]
        public string Colour { get; set; }

        [MaxLength(30)]
        public string Manufacturer { get; set; }

        [MaxLength(60)]
        public string Model { get; set; }

        [Range(2, 18)]
        [Display(Name = "Number of Wheels")]
        public int NumberOfWheels { get; set; }

    }
}
