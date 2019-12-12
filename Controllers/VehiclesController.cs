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

namespace Garage2.Controllers
{
    public class VehiclesController:Controller
    {
        private GarageContext _context;

        public VehiclesController(GarageContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Contracts.ToListAsync();
            return View(vehicles);
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
                vehicle.RegistrationNumber = viewModel.RegistrationNumber;
                vehicle.Type = viewModel.Type;
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

        public async Task<IActionResult> FilterByRegNum(string RegNum, int? type)
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
