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
        private static int nextSpotId = 0;

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
            if (spot == null || spot.VehicleCount == 0 || (forMotorcycle && spot.HasMotorcycles && spot.VehicleCount < 3))
            {
                if (spot == null)
                {
                    spot = new ParkSpot(nextSpotId);
                    parkSpots[nextSpotId] = FirstAvailableSpot = spot;
                    nextSpotId++;
                }
                FirstAvailableSpot = spot;
                return true;
            }
            else
                return false;
        }

        public static bool SpotIsAvailable(ParkSpot[] spots, bool forMotorcycle)
        {
            foreach (var spot in spots)
            {
                if (SpotIsAvailable(spot, forMotorcycle))
                {
                    return true;
                }
            }
            return false;
        }

        public static ParkSpot GetAvailableSpot(ParkSpot[] spots, bool forMotorcycle)
        {
            if (FirstAvailableSpot != null && SpotIsAvailable(FirstAvailableSpot, forMotorcycle))
                return FirstAvailableSpot;
            if (SpotIsAvailable(spots, forMotorcycle))
                    return FirstAvailableSpot;
            return null;
        }

        public static ParkSpot FindSpotByVehicle(ParkedVehicle vehicle)
        {
            foreach (var spot in parkSpots)
            {
                if (spot != null && spot.ParkedVehicles.Contains(vehicle))
                    return spot;
            }
            return null;
        }

    }
}