using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Models
{
    public class ParkSpot
    {
        public ParkedVehicle[] ParkedVehicles { get; set; }
        public bool HasMotorcycles { get; set; }
        public int VehicleCount { get; set; }
    }
}
