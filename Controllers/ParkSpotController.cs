using Garage2.Data;
using Garage2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Controllers
{
    public class ParkSpotController : Controller
    {
        private GarageContext _context;
        private IConfiguration _configuration;
        private ParkSpot[] parkSpots;
        private bool parkSpotsInitialized;

        public ParkSpotController(GarageContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            parkSpots = new ParkSpot[int.Parse(configuration["ParkingSpaces"])];
            parkSpotsInitialized = false;
        }

        public IActionResult Index()
        {
            if (!parkSpotsInitialized)
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
                var spot = new ParkSpot();
                spot.ParkedVehicles = new ParkedVehicle[3];
                spot.ParkedVehicles[0] = vehicles[i];
                spot.VehicleCount = 1;
                parkSpots[i] = spot;
            }
            parkSpotsInitialized = true;
        }
    }
}
