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

namespace Garage2.Controllers
{
    public class VehiclesController:Controller
    {
        private GarageContext _context;

        public VehiclesController(GarageContext context)
        {
            _context = context;
        }

        public string regnum { get; set; }
        public string Type { get; set; }
        public string Manufacturere { get; set; }
        public string color { get; set; }
        public string model { get; set; }


        // GET: Vehicle/Details
        public async Task<IActionResult> Details(string RegNum)
        {
            //RegNum = "PAY276";


            if (RegNum == null)
            {
                return NotFound();

            }

            var models = await _context.ParkedVehicles
                .FirstOrDefaultAsync(m => m.RegistrationNumber == RegNum);

            if (models == null)
            {
                return NotFound();
            }

            var ModelSum = new VehicaleSummaryDetailsViewModel(models);
            ModelSum.Colour = models.Colour;
            ModelSum.RegistrationNumber = models.RegistrationNumber;
            ModelSum.Manufacturer = models.Manufacturer;
            ModelSum.Model = models.Model;
            ModelSum.Type = models.Type;
           
            return View(ModelSum);

        }



        //public IActionResult Details()
        //{
        //}
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

            var contracts = _context.Contracts.Where(c => vehicles.Contains(c.Vehicle));
            var viewModels = new List<VehicleSummaryViewModel>();

            foreach (ParkedVehicle vehicle in vehicles)
            {
                var parkingDate = contracts.FirstOrDefault(c => c.Vehicle.RegistrationNumber
                                                    == vehicle.RegistrationNumber);
                if (parkingDate == null)
                {
                    //Something has gone wrong
                    throw new ApplicationException("ParkingContract for a ParkedVehicle not found");
                }

                VehicleSummaryViewModel viewModel = CreateSummaryViewModel(vehicle, parkingDate);

                viewModels.Add(viewModel);
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
                default:
                    viewModels = viewModels.OrderBy(s => s.Type).ToList();
                    break;
            }

            return View(viewModels);
        }

        private static VehicleSummaryViewModel CreateSummaryViewModel(ParkedVehicle vehicle, ParkingContract parkingDate)
        {
            var model = new VehicleSummaryViewModel();
            model.Colour = vehicle.Colour;
            model.RegistrationNumber = vehicle.RegistrationNumber;
            model.ParkingTime = parkingDate.ParkingDate;
            model.Type = vehicle.Type;
            return model;
        }

        // Get: Vehicle/Park
        public IActionResult Park()
        {
            return View();

        }

        // Post: Vehicle/Park
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Park( ParkParkedVehicleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var vehicle = new ParkedVehicle();
                //populate all fields from viewModel
                vehicle.RegistrationNumber = viewModel.RegistrationNumber;
                vehicle.Type = viewModel.Type;
                vehicle.Colour = viewModel.Colour;
                vehicle.Manufacturer = viewModel.Manufacturer;
                vehicle.Model = viewModel.Model;
                vehicle.NumberOfWheels = viewModel.NumberOfWheels;

                _context.Add(vehicle);
                _context.Add(new ParkingContract()
                {
                    Vehicle = vehicle,
                    ParkingDate=DateTime.Today
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();

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
            var contract =  await _context.Contracts.FirstOrDefaultAsync(c => c.Vehicle == vehicle);
            if (contract != null && contract != null)
            {
                _context.Contracts.Remove(contract);
                _context.ParkedVehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }

            TempData["vehicle"] = JsonConvert.SerializeObject(vehicle);
            TempData["contract"] = JsonConvert.SerializeObject(contract);
            TempData.Keep();
            return RedirectToAction(nameof(ParkingReceipt));

        }

        public IActionResult ParkingReceipt()
        {
            var vehicleString = TempData["vehicle"] as string;
            var contractString = TempData["contract"] as string;
            var vehicle = JsonConvert.DeserializeObject<ParkedVehicle>(vehicleString) as ParkedVehicle;
            var contract = JsonConvert.DeserializeObject<ParkingContract>(contractString) as ParkingContract;
            if (contract == null || vehicle == null)
            {
                throw new Exception("JsonConvert failed to convert TempData");
            }
            var currentTime = DateTime.Now;
            var parkingDuration = currentTime - contract.ParkingDate;
            var model = new Tuple<ParkedVehicle, ParkingContract,DateTime, TimeSpan>(vehicle, contract, currentTime, parkingDuration);

            return View(model);
        } 

        // Filter by RegNum
        public async Task<IActionResult> Filter(string RegNum, int? type)
        {
            var model = string.IsNullOrWhiteSpace(RegNum) ?
                await _context.ParkedVehicles.ToListAsync() :
                await _context.ParkedVehicles.Where(m => m.RegistrationNumber == RegNum).ToListAsync();

            model = type == null ?
                model :
                model.Where(m => m.Type == (VehicleType)type).ToList();

            return View(model);

        }

        public async Task<IActionResult> Sort(string sortOrder)
        {
            ViewBag.TypeSortParm = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewBag.RegistrationNumberSortParm = sortOrder == "RegistrationNumber" ? "RegistrationNumber_desc" : "RegistrationNumber";
            ViewBag.ColourSortParm = sortOrder == "Colour" ? "Colour_desc" : "Colour";
            ViewBag.ParkingTimeSortParm = sortOrder == "ParkingTime" ? "ParkingTime_desc" : "ParkingTime";

            var models = from s in _context.ParkedVehicles
                          select s;

            switch (sortOrder)
            {
                case "type_desc":
                    models = models.OrderByDescending(s => s.Type);
                    break;
                case "RegistrationNumber":
                    models = models.OrderBy(s => s.RegistrationNumber);
                    break;
                case "RegistrationNumber_desc":
                    models = models.OrderByDescending(s => s.RegistrationNumber);
                    break;
                case "ParkingTime":
                    //vehicle = vehicle.OrderBy(s => s.ParkingTime);
                    break;
                case "ParkingTime_desc":
                    //vehicle = vehicle.OrderByDescending(s => s.ParkingTime);
                    break;
                case "Colour":
                    models = models.OrderBy(s => s.Colour);
                    break;
                case "Colour_desc":
                    models = models.OrderByDescending(s => s.Colour);
                    break;
                default:
                    models = models.OrderBy(s => s.Type);
                    break;
            }
            return View( models.ToList());

     

        }



    }
}
