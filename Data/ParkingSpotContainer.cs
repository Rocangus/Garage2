using Garage2.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Data
{
    public class ParkingSpotContainer
    {
        private static ParkSpot[] parkSpots;
        public static bool IsInitialized { get; set; }

        public static ParkSpot[] GetParkSpots(IConfiguration configuration)
        {
            if (parkSpots == null)
            {
                parkSpots = new ParkSpot[int.Parse(configuration["ParkingSpaces"])];
                IsInitialized = false;
            }
            return parkSpots;
        }

        public static bool SpotIsAvailable(ParkSpot spot, bool forMotorcycle)
        {
            if (spot.VehicleCount == 0 || (forMotorcycle && spot.HasMotorcycles && spot.VehicleCount < 3))
                return true;
            else
                return false;
        }

        public static bool SpotIsAvailable(ParkSpot[] spots, bool forMotorcycle)
        {
            foreach (var spot in spots)
            {
                if (SpotIsAvailable(spot, forMotorcycle))
                    return true;
            }
            return false;
        }

        public static ParkSpot GetAvailableSpot(ParkSpot[] spots, bool forMotorcycle)
        {
            foreach (var spot in spots)
            {
                if (SpotIsAvailable(spot, forMotorcycle))
                    return spot;
            }
            return null;
        }
    }
}