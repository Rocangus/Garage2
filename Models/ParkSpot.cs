using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Models
{
    public class ParkSpot
    {
        public int Id { get; set; }
        public ParkedVehicle[] ParkedVehicles { get; set; }
        public bool HasMotorcycles { get; set; }
        public int VehicleCount { get; set; }

        public ParkSpot(int id)
        {
            Id = id;
            ParkedVehicles = new ParkedVehicle[3];
            HasMotorcycles = false;
            VehicleCount = 0;
        }

        public bool Park(ParkedVehicle vehicle)
        {
            for (var i = 0; i < ParkedVehicles.Length; i++)
            {
                var v = ParkedVehicles[i];
                if (v == null)
                {
                    ParkedVehicles[i] = vehicle;
                    return true;
                }
            }
            return false;
        }

        public bool Unpark(ParkedVehicle vehicle)
        {
            for (var i = 0; i < ParkedVehicles.Length; i++)
            {
                var v = ParkedVehicles[i];
                if (v != null && v.Equals(vehicle))
                {
                    ParkedVehicles[i] = null;
                    VehicleCount--;
                    return true;
                }
            }
            return false;
        }
    }
}
