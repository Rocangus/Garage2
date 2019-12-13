using Microsoft.AspNetCore.Mvc;
using System;
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




        // GET: Vehicle/Details
        public async Task<IActionResult> Details(string RegNum)
        {
            RegNum = "PAY276";


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

            var T = new VehicaleSummaryDetailsViewModel(models);
            return View(T);

        }


    



        //public IActionResult Details()
        //{
        //}
        public async Task<IActionResult> Index()
        {
            var contracts = await _context.Contracts.ToListAsync();
            var parkvehecle = await _context.ParkedVehicles.ToListAsync();
            var models = new List<VehicleSummaryViewModel>();

            foreach (ParkedVehicle vehicle in parkvehecle)
            {
                var parkingDate = contracts.FirstOrDefault(c => c.Vehicle.RegistrationNumber
                                                    == vehicle.RegistrationNumber);
                if (parkingDate == null)
                {
                    //Something has gone wrong
                    throw new ApplicationException("ParkingContract for a ParkedVehicle not found");
                }

                VehicleSummaryViewModel model = CreateSummaryViewModel(vehicle, parkingDate);

                models.Add(model);
            }

            return View(models);
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

            return View( model);

        }

    }
}
