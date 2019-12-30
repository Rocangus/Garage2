using Garage2.Data;
using Garage2.Models;
using Garage2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Controllers
{
    public class MembersController: Controller
    {

        private GarageContext _context;
        public MembersController(GarageContext context)
        {
            _context = context;
            
        }

        public async Task<IActionResult> Index()
        {
            var models = new List<MemberSummaryViewModel>();

            var members = await _context.Members.ToListAsync();
            var vehicles = await _context.ParkedVehicles.ToListAsync();
            foreach (var member in members)
            {
                var model = new MemberSummaryViewModel
                {
                    FirstName = member.FirstName,
                    LastName = member.LastName
                };
                member.OwnedVehicles = vehicles.Where(v => v.MemberId == member.MemberId).ToList();
                model.Count = member.OwnedVehicles.Count;
                models.Add(model);
            }

            return View(models);
        }

        // Get: Members/ParkEmail
        public IActionResult CheckEmail()
        {
            return View();
        }

        // Post: Members/ParkEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckEmail(EmailAddress emailAddress)
        {
            var results = _context.Members.Where(m => m.Email == emailAddress.Email);
            if (results.Any())
            {
                return RedirectToAction(nameof(VehiclesController.Park), "Vehicles");
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Register()

        {

            return View();
        }

        public IActionResult Register(MemberViewModel model)
        {
            //var member = _context.
            return RedirectToAction(nameof(Index));

        }
    }
}
