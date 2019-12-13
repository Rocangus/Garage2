using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Models
{
    
    public class ParkingContract
    {
        public int Id { get; set; }
        public ParkedVehicle Vehicle { get; set; }
        [Required]
        public DateTime ParkingDate { get; set; }
    }
}
