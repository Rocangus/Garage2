using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Models
{
    public class ParkedVehicle
    {
        [Key]
        [RegularExpression(@"^([A-H,J-P,R-Z]|[a-h,j-p,r-z]){3}(\d{2}([A-H,J-P,R-Z]|[a-h,j-p,r-z]){1}|\d{3})", 
            ErrorMessage = "The specified registration number is not valid in Sweden.")]
        [Remote("ValidateRegistrationNumber", "Vehicles")]
        [Required]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Required]
        public int VehicleTypeId { get; set; }

        public VehicleType Type { get; set; }

        [MaxLength(15)]
        [Required]
        public string Colour { get; set; }

        [MaxLength(30)]
        public string Manufacturer { get; set; }

        [MaxLength(60)]
        public string Model { get; set; }

        public int MemberId { get; set; }

        [Range(2, 18)]
        [Display(Name = "Number of Wheels")]
        public int NumberOfWheels { get; set; }

        public DateTime ParkingDate { get; set; }

        public bool IsParked { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is ParkedVehicle vehicle)
                return RegistrationNumber.Equals(vehicle.RegistrationNumber);
            return false;
        }
    }
}
