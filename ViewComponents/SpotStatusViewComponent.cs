using Garage2.Data;
using Garage2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.ViewComponents
{
    public class SpotStatusViewComponent : ViewComponent
    {
        private readonly GarageContext _context;
        private readonly IConfiguration _configuration;
        private ParkSpot[] parkSpots;

        public SpotStatusViewComponent(GarageContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            parkSpots = ParkingSpotContainer.GetParkSpots(configuration);
        }

        public IViewComponentResult Invoke()
        {
            if (!ParkingSpotContainer.IsInitialized)
            {
                InitializeParkSpots();
            }
            System.Tuple<IEnumerable<ParkSpot>, int> model = new Tuple<IEnumerable<ParkSpot>, int>(parkSpots, int.Parse(_configuration["ParkingSpaces"]));
            return View(model);
        }

        private void InitializeParkSpots()
        {
            var vehicles = _context.ParkedVehicles.ToList();
            for (var i = 0; i < vehicles.Count(); i++)
            {
                var vehicleIsMotorcycle = vehicles[i].Type == VehicleType.Motorcycle;
                var spot = ParkingSpotContainer.GetAvailableSpot(parkSpots, vehicleIsMotorcycle);

                spot.Park(vehicles[i]);
                spot.VehicleCount += 1;
                spot.HasMotorcycles = vehicleIsMotorcycle;
                parkSpots[spot.Id] = spot;
            }
            ParkingSpotContainer.IsInitialized = true;
        }
    }
}
