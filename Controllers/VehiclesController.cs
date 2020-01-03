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
using Garage2.Extensions;

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
            ModelSum.Type = await _context.VehicleTypes.FirstOrDefaultAsync(t => t.Id == vehicle.VehicleTypeId);
            ModelSum.ParkingTime = vehicle.ParkingDate - DateTime.Now;
            // ModelSum.NumberOfWheels = vehicle.NumberOfWheels;

            return View((ModelSum, fromMember));

        }

        public async Task<IActionResult> Index(string RegNum, int? type, string sortOrder)
        {
            ViewBag.TypeSortParm = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewBag.RegistrationNumberSortParm = sortOrder == "RegistrationNumber" ? "RegistrationNumber_desc" : "RegistrationNumber";
            ViewBag.ColourSortParm = sortOrder == "Colour" ? "Colour_desc" : "Colour";
            ViewBag.ParkingTimeSortParm = sortOrder == "ParkingTime" ? "ParkingTime_desc" : "ParkingTime";
            ViewBag.OwnerNameSortParm = sortOrder == "OwnerName" ? "OwnerName_desc" : "OwnerName";



            var vehicles = string.IsNullOrWhiteSpace(RegNum) ?
                _context.ParkedVehicles :
                _context.ParkedVehicles.Where(m => m.RegistrationNumber.Contains(RegNum));

            IEnumerable<SelectListItem> vehicleTypeSelectItems = await GetVehicleTypeSelectListItems();

            vehicles = type == null ?
                vehicles :
                vehicles.Where(m => m.VehicleTypeId == type);

            var viewModels = new List<VehicleSummaryViewModel>();
            IEnumerable<Member> members = _context.Members;

            foreach (ParkedVehicle vehicle in vehicles)
            {
                if (vehicle.IsParked)
                {
                    VehicleSummaryViewModel viewModel = CreateSummaryViewModel(vehicle, members);
                    viewModels.Add(viewModel);
                }
            }

            switch (sortOrder)
            {
                case "type_desc":
                    viewModels = viewModels.OrderByDescending(s => s.Type).ToList();
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
                case "OwnerName":
                    viewModels = viewModels.OrderBy(s => s.OwnerName).ToList();
                    break;
                case "OwnerName_desc":
                    viewModels = viewModels.OrderByDescending(s => s.OwnerName).ToList();
                    break;
                default:
                    viewModels = viewModels.OrderBy(s => s.Type).ToList();
                    break;
            }

            return View(new Tuple<IEnumerable<VehicleSummaryViewModel>, IEnumerable<SelectListItem>>
                                 (viewModels, vehicleTypeSelectItems));

        }

        private async Task<List<SelectListItem>> GetVehicleTypeSelectListItems()
        {
            var vehicleTypeSelectItems = new List<SelectListItem>();
            var vehicleTypes = await _context.VehicleTypes.ToListAsync();
            foreach (VehicleType vehicleType in vehicleTypes)
            {
                var item = new SelectListItem()
                {
                    Value = vehicleType.Id.ToString(),
                    Text = vehicleType.Name
                };
                vehicleTypeSelectItems.Add(item);
            }

            return vehicleTypeSelectItems;
        }

        private static VehicleSummaryViewModel CreateSummaryViewModel(ParkedVehicle vehicle, IEnumerable<Member> members)
        {
            var model = new VehicleSummaryViewModel();
            var owner = members.FirstOrDefault(m => m.MemberId == vehicle.MemberId);
            model.Colour = vehicle.Colour;
            model.RegistrationNumber = vehicle.RegistrationNumber;
            model.ParkingTime = DateTime.Now - vehicle.ParkingDate;
            model.Type = vehicle.Type;
            model.OwnerName = owner.FirstName + " " + owner.LastName;
            return model;
        }


        // Get: Vehicle/Park
        public async Task<IActionResult> Park()
        {
            TempData.Keep();
            ViewData["selectItems"] = await GetVehicleTypeSelectListItems();
            return View();

        }

        // Post: Vehicle/Park
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Park(ParkParkedVehicleViewModel viewModel)
        {
            var parkSpots = ParkingSpotContainer.GetParkSpots(_configuration);
            if (ModelState.IsValid)
            {
                var vehicle = new ParkedVehicle();
                vehicle.VehicleTypeId = int.Parse(Request.Form["Type"].ToString());
                if (vehicle.VehicleTypeId == 0)
                {
                    throw new ArgumentException("The value of the SelectItem selected was zero.");
                }
               
                var member = TempDataExtensions.Get<Member>(TempData, "member");
                vehicle.MemberId = member.MemberId;
                PopulateVehicleFromViewModel(viewModel, vehicle);
                await ParkVehicleInBackend(parkSpots, vehicle);
                vehicle.IsParked = true;
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();

        }

        private async Task ParkVehicleInBackend(ParkSpot[] parkSpots, ParkedVehicle vehicle)
        {
            var vehicleType = await _context.VehicleTypes.Where(v => v.Id == vehicle.VehicleTypeId).FirstOrDefaultAsync();
            int numberRequired = GetRequiredNumberOfSpots(vehicleType);
            int startOfSpotSequence;
            if (vehicleType.Name == "Motorcycle")
            {
                {
                    var spot = ParkingSpotContainer.GetAvailableSpot(parkSpots, true);
                    if (spot != null)
                    {
                        spot.Park(vehicle);
                        spot.VehicleCount++;
                        spot.HasMotorcycles = true;
                    }
                }
            }
            else if (vehicleType.Name != "Motorcycle" && ParkingSpotContainer.FindConsecutiveSpots(numberRequired, out startOfSpotSequence))
            {
                ParkingSpotContainer.ParkOnMultipleSpots(startOfSpotSequence, numberRequired, vehicle);

            }
        }

        public IActionResult RePark()
        {
            TempData.Keep();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RePark(string registrationNumber)
        {
            var vehicle = await _context.ParkedVehicles.FirstOrDefaultAsync(v => v.RegistrationNumber.Equals(registrationNumber));
            if (vehicle == null)
            {
                TempData.Keep();
                return RedirectToAction(nameof(Park));
            }
            vehicle.ParkingDate = DateTime.Now;
            vehicle.IsParked = true;
            var parkSpots = ParkingSpotContainer.GetParkSpots(_configuration);
            await ParkVehicleInBackend(parkSpots, vehicle);
            try
            {
                _context.Update(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(actionName: "Index", controllerName: "Vehicles");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.ParkedVehicles.FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber) == null)
                {
                    TempData.Keep();
                    return RedirectToAction(nameof(Park));
                }
                else
                {
                    throw;
                }
            }
            return View();
        }

        private static int GetRequiredNumberOfSpots(VehicleType vehicleType)
        {
            int numberRequired;
            switch (vehicleType.Name)
            {
                case "Bus":
                    {
                        numberRequired = 2;
                        break;
                    }
                case "Truck":
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

            return numberRequired;
        }

        private static void PopulateVehicleFromViewModel(ParkParkedVehicleViewModel viewModel, ParkedVehicle vehicle)
        {
            vehicle.RegistrationNumber = viewModel.RegistrationNumber;
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

            var ModelSum = new VehicleSummaryUnparkViewModel(vehicle);
            ModelSum.RegistrationNumber = vehicle.RegistrationNumber;
            ModelSum.Manufacturer = vehicle.Manufacturer;
            ModelSum.Model = vehicle.Model;
            ModelSum.Type = await _context.VehicleTypes.FirstOrDefaultAsync(t => t.Id == vehicle.VehicleTypeId);
         

            return View(ModelSum);

        }

        // POST: Vehicle/UnPark
        [HttpPost, ActionName("UnPark")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnParkConfirmed(string RegNum)
        {
            var vehicle = await _context.ParkedVehicles.FindAsync(RegNum);
            var vehicleType = await _context.VehicleTypes.FirstOrDefaultAsync(t => t.Id == vehicle.VehicleTypeId);
            var spotsRequired = GetRequiredNumberOfSpots(vehicleType);
            if (vehicle != null)
            {
                vehicle.IsParked = false;
                _context.Update(vehicle);
                await _context.SaveChangesAsync();
            } else { throw new ArgumentNullException(nameof(vehicle), "The vehicle could not be found in the database."); }

            ParkSpot spot;
            // TODO: More elegant solution without extra calls to FindSpotByVehicle
            for (var i = 0; i < spotsRequired; i++)
            {
                spot = ParkingSpotContainer.FindSpotByVehicle(vehicle);
                spot.Unpark(vehicle);
            }

            TempDataExtensions.Set(TempData, "vehicle", vehicle);
            TempData.Keep();
            return RedirectToAction(nameof(ParkingReceipt));

        }

        public IActionResult ParkingReceipt()
        {
            var vehicle = TempDataExtensions.Get<ParkedVehicle>(TempData, "vehicle");
            if (vehicle == null)
            {
                throw new Exception("JsonConvert failed to convert TempData");
            }
            DateTime currentTime;
            TimeSpan parkingDuration;
            decimal cost;
            GetParkingCost(vehicle, out currentTime, out parkingDuration, out cost);
            var member = _context.Members.FirstOrDefault(m => m.MemberId == vehicle.MemberId);
            var memberFullName = member.FullName;
          

           var model = new Tuple<ParkedVehicle, DateTime, TimeSpan, decimal, string>(vehicle, currentTime, parkingDuration, cost, memberFullName);

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

            var vehicleTypes = _context.VehicleTypes;
            var typeCounts = new Dictionary<VehicleType, int>();
            foreach (var type in vehicleTypes)
            {
                var count = vehicles.Where(v => v.VehicleTypeId == type.Id).Count();
                if(count > 0)
                {
                    typeCounts.Add(type, count);
                }
                   
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

        public IActionResult ValidateRegistrationNumber(string registrationNumber)
        {
            if (_context.ParkedVehicles.Any(m => m.RegistrationNumber == registrationNumber))
            {
                return Json($"The vehicle with registration number {registrationNumber} is already in the garage.");
            }
            return Json(true);
        }
    }
}
