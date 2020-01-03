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
        private static int nextSpotId = 0;
        private static int lastFinalIndex;
        private static int lastSequenceStart;
        private static ParkSpot AvailableSpot;

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
                    parkSpots[nextSpotId] = spot;
                    nextSpotId++;
                }
                AvailableSpot = spot;
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
            if (SpotIsAvailable(spots, forMotorcycle))
                return AvailableSpot;
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

        public static bool FindConsecutiveSpots(int numberRequired, out int startOfSpotSequence)
        {
            lastFinalIndex = 0;
            var spot = GetAvailableSpot(parkSpots, false);
            lastFinalIndex = spot.Id;
            while (lastFinalIndex < 25)
            {
                if (FindConsecutiveSpots(lastFinalIndex, numberRequired))
                {
                    startOfSpotSequence = lastSequenceStart;
                    return true;
                }

            }
            startOfSpotSequence = -1;
            return false;
        }

        private static bool FindConsecutiveSpots(int start, int numberRequired)
        {
            lastSequenceStart = start;
            var i = start;
            var numberAvailable = 0;
            while (i < parkSpots.Length && SpotIsAvailable(parkSpots[i], false) && numberAvailable < numberRequired)
            {
                numberAvailable++;
                i++;
            }
            lastFinalIndex = i;
            return numberAvailable >= numberRequired;
        }

        public static ParkSpot[] ParkOnMultipleSpots(int startOfSpotSequence, int spotSequenceLength, ParkedVehicle vehicle)
        {
            var spots = new ParkSpot[spotSequenceLength];
            var n = 0;
            for (var i = startOfSpotSequence; i < startOfSpotSequence + spotSequenceLength; i++)
            {
                parkSpots[i].Park(vehicle);
                parkSpots[i].VehicleCount = 1;
                spots[n] = parkSpots[i];
                n++;
            }
            return spots;
        }
    }
}