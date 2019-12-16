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
        private static ParkSpot FirstAvailableSpot;

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
                {
                    FirstAvailableSpot = spot;
                    return true;
                }
            }
            return false;
        }

        public static ParkSpot GetAvailableSpot(ParkSpot[] spots, bool forMotorcycle)
        {
            if (SpotIsAvailable(FirstAvailableSpot, forMotorcycle))
                return FirstAvailableSpot;
            if (SpotIsAvailable(spots, forMotorcycle))
                    return FirstAvailableSpot;
            return null;
        }

        public static ParkSpot FindSpotByVehicle(ParkedVehicle vehicle)
        {
            foreach (var spot in parkSpots)
            {
                if (spot != null && spot.ParkedVehicles.Any(v => v.RegistrationNumber == vehicle.RegistrationNumber))
                    return spot;
            }
            return null;
        }

    }
}