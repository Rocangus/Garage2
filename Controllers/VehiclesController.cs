using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Garage2.Models;
using Garage2.Data;
using Garage2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Garage2.Controllers
{
    public class VehiclesController : Controller
    {
        private GarageContext _context;
        private decimal minutePrice = 0.05M;
        private IConfiguration _configuration;

        public VehiclesController(GarageContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        // GET: Vehicle/Details
        public async Task<IActionResult> Details(string RegNum, bool fromMember)
        {
            //RegNum = "PAY276";

            if (RegNum == null)
            {
                return NotFound();

            }
            //
            var vehicle = await _context.ParkedVehicles
                .FirstOrDefaultAsync(v => v.RegistrationNumber == RegNum);

            if (vehicle == null)
            {
                return NotFound();
            }


            var ModelSum = new VehicaleSummaryDetailsViewModel(vehicle);
            ModelSum.Colour = vehicle.Colour;
            ModelSum.RegistrationNumber = vehicle.RegistrationNumber;
            ModelSum.Manufacturer = vehicle.Manufacturer;
            ModelSum.Model = vehicle.Model;
            ModelSum.Type = vehicle.Type;
            ModelSum.ParkingTime = vehicle.ParkingDate - DateTime.Now;
            // ModelSum.NumberOfWheels = vehicle.NumberOfWheels;

            return View(( ModelSum, fromMember ));

        }

        public async Task<IActionResult> Index(string RegNum, int? type, string sortOrder)
        {
            ViewBag.TypeSortParm = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewBag.RegistrationNumberSortParm = sortOrder == "RegistrationNumber" ? "RegistrationNumber_desc" : "RegistrationNumber";
            ViewBag.ColourSortParm = sortOrder == "Colour" ? "Colour_desc" : "Colour";
            ViewBag.ParkingTimeSortParm = sortOrder == "ParkingTime" ? "ParkingTime_desc" : "ParkingTime";



            var vehicles = string.IsNullOrWhiteSpace(RegNum) ?
                _context.ParkedVehicles :
                _context.ParkedVehicles.Where(m => m.RegistrationNumber.Contains(RegNum));

            vehicles = type == null ?
                vehicles :
                vehicles.Where(m => m.Type == (VehicleType)type);

            var viewModels = new List<VehicleSummaryViewModel>();

            foreach (ParkedVehicle vehicle in vehicles)
            {

                VehicleSummaryViewModel viewModel = CreateSummaryViewModel(vehicle);

                viewModels.Add(viewModel);
            }

            switch (sortOrder)
            {
                case "type_desc":
                    viewModels =  viewModels.OrderByDescending(s => s.Type).ToList();
                    break;
                case "RegistrationNumber":
                    viewModels = viewModels.OrderBy(s => s.RegistrationNumber).ToList();
                    break;
                case "RegistrationNumber_desc":
                    viewModels = viewModels.OrderByDescending(s => s.RegistrationNumber).ToList();
                    break;
                case "ParkingTime":
                    viewModels = viewModels.OrderBy(s => s.ParkingTime).ToList();
                    break;
                case "ParkingTime_desc":
                    viewModels = viewModels.OrderByDescending(s => s.ParkingTime).ToList();
                    break;
                case "Colour":
                    viewModels = viewModels.OrderBy(s => s.Colour).ToList();
                    break;
                case "Colour_desc":
                    viewModels = viewModels.OrderByDescending(s => s.Colour).ToList();
                    break;
                default:
                    viewModels = viewModels.OrderBy(s => s.Type).ToList();
                    break;
            }

            return View(viewModels);

        }

        private static VehicleSummaryViewModel CreateSummaryViewModel(ParkedVehicle vehicle)
        {
            var model = new VehicleSummaryViewModel();
            model.Colour = vehicle.Colour;
            model.RegistrationNumber = vehicle.RegistrationNumber;
            model.ParkingTime = DateTime.Now - vehicle.ParkingDate;
            model.Type = vehicle.Type;
            return model;
        }


        // Get: Vehicle/Park
        public IActionResult Park()
        {
            TempData.Keep();
            return View();

        }

        // Post: Vehicle/Park
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Park(ParkParkedVehicleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var vehicle = new ParkedVehicle();
                var member = await _context.Members.Where(m => m.Email.Equals(TempData["Email"] as string)).FirstOrDefaultAsync();
                vehicle.MemberId = member.MemberId;
                int numberRequired;
                switch (viewModel.Type)
                {
                    case VehicleType.Bus:
                        {
                            numberRequired = 2;
                            break;
                        }
                    case VehicleType.Truck:
                        {
                            numberRequired = 3;
                            break;
                        }
                    default:
                        {
                            numberRequired = 1;
                            break;
                        }

                }
                int startOfSpotSequence;
                if (viewModel.Type == VehicleType.Motorcycle)
                {
                    {
                        var spot = ParkingSpotContainer.GetAvailableSpot(ParkingSpotContainer.GetParkSpots(_configuration), true);
                        if (spot != null)
                        {
                            PopulateVehicleFromViewModel(viewModel, vehicle);
                            spot.Park(vehicle);
                            spot.VehicleCount++;
                            spot.HasMotorcycles = true;
                        }
                    }
                }
                else if (viewModel.Type != VehicleType.Motorcycle && ParkingSpotContainer.FindConsecutiveSpots(numberRequired, out startOfSpotSequence))
                {
                    PopulateVehicleFromViewModel(viewModel, vehicle);
                    ParkingSpotContainer.ParkOnMultipleSpots(startOfSpotSequence, numberRequired, vehicle);

                }
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();

        }

        private static void PopulateVehicleFromViewModel(ParkParkedVehicleViewModel viewModel, ParkedVehicle vehicle)
        {
            vehicle.RegistrationNumber = viewModel.RegistrationNumber;
            vehicle.Type = viewModel.Type;
            vehicle.Colour = viewModel.Colour;
            vehicle.Manufacturer = viewModel.Manufacturer;
            vehicle.Model = viewModel.Model;
            vehicle.NumberOfWheels = viewModel.NumberOfWheels;
            vehicle.ParkingDate = DateTime.Now;
        }

        // GET: Vehicle/UnPark
        public async Task<IActionResult> UnPark(string RegNum)
        {
            if (RegNum == null)
            {
                return NotFound();

            }

            var vehicle = await _context.ParkedVehicles
                .FirstOrDefaultAsync(m => m.RegistrationNumber == RegNum);

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);

        }

        // POST: Vehicle/UnPark
        [HttpPost, ActionName("UnPark")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnParkConfirmed(string RegNum)
        {
            var vehicle = await _context.ParkedVehicles.FindAsync(RegNum);
            if (vehicle != null)
            {
                _context.ParkedVehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }

            ParkSpot spot;
            // TODO: More elegant solution without extra calls to FindSpotByVehicle
            do
            {
                spot = ParkingSpotContainer.FindSpotByVehicle(vehicle);
                spot.Unpark(vehicle);
            } while (ParkingSpotContainer.FindSpotByVehicle(vehicle) != null);

            TempData["vehicle"] = JsonConvert.SerializeObject(vehicle);
            TempData.Keep();
            return RedirectToAction(nameof(ParkingReceipt));

        }

        public IActionResult ParkingReceipt()
        {
            var vehicleString = TempData["vehicle"] as string;
            var vehicle = JsonConvert.DeserializeObject<ParkedVehicle>(vehicleString) as ParkedVehicle;
            if (vehicle == null)
            {
                throw new Exception("JsonConvert failed to convert TempData");
            }
            DateTime currentTime;
            TimeSpan parkingDuration;
            decimal cost;
            GetParkingCost(vehicle, out currentTime, out parkingDuration, out cost);

            var model = new Tuple<ParkedVehicle, DateTime, TimeSpan, decimal>(vehicle, currentTime, parkingDuration, cost);

            return View(model);
        }

        private void GetParkingCost(ParkedVehicle vehicle, out DateTime currentTime, out TimeSpan parkingDuration, out decimal cost)
        {
            currentTime = DateTime.Now;
            parkingDuration = currentTime - vehicle.ParkingDate;
            cost = 50 * parkingDuration.Days;
            var durationWithDaysRemoved = parkingDuration - new TimeSpan(parkingDuration.Days * TimeSpan.TicksPerDay);
            cost += (decimal)durationWithDaysRemoved.TotalMinutes * minutePrice;
        }

        public async Task<IActionResult> GetStatistics()
        {
            // How many vehicle in vary types
            var vehicles = await _context.ParkedVehicles.ToArrayAsync();

            var vehicleTypes = vehicles.Select(v => v.Type).Distinct();
            var typeCounts = new Dictionary<VehicleType, int>();
            foreach (var type in vehicleTypes)
            {
                var count = vehicles.Where(v => v.Type == type).Count();
                typeCounts.Add(type, count);
            }

            int Wheel = vehicles.Sum(v => v.NumberOfWheels);

            //Total Cost
            decimal TotalCost = GetTotalParkingCost(vehicles);

            //How many vehicles with color 
            var colours = vehicles.Select(v => v.Colour).Distinct();
            var colourCounts = new Dictionary<string, int>();
            foreach (var colour in colours)
            {
                var count = vehicles.Where(v => v.Colour == colour).Count();
                colourCounts.Add(colour, count);
            }

            var model = new Tuple<Dictionary<VehicleType, int>, int, decimal, Dictionary<string, int>>(typeCounts, Wheel, TotalCost, colourCounts);

            return View(model);
        }

        private decimal GetTotalParkingCost(ParkedVehicle[] vehicles)
        {
            var TotalCost = 0.0M;
            var currentTime = DateTime.Now;
            for (int i = 0; i < vehicles.Length; i++)
            {
                var contract = vehicles[i];
                TimeSpan parkingDuration;
                decimal cost;
                GetParkingCost(contract, out currentTime, out parkingDuration, out cost);
                TotalCost += cost;

            }

            return TotalCost;
        }

    }
}
