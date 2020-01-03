using Garage2.Data;
using Garage2.Models;
using Garage2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Garage2.Extensions;

namespace Garage2.Controllers
{

    public class MembersController: Controller
    {
        private readonly IMapper mapper;
        private GarageContext _context;
        public MembersController(GarageContext context, IMapper mapper)
        {
            this.mapper = mapper;
            _context = context;
            
        }

        public async Task<IActionResult> Index(string name)
        {
            var search = string.IsNullOrWhiteSpace(name) ?
               await _context.Members.ToListAsync ()  :
               await _context.Members.Where(m => (m.FirstName + " " + m.LastName).Contains(name)).ToListAsync();

            var models = new List<MemberSummaryViewModel>();
            var vehicles = await _context.ParkedVehicles.ToListAsync();
            foreach (var member in search)
            {
                var model = new MemberSummaryViewModel
                {
                    Id = member.MemberId,
                    FirstName = member.FirstName,
                    LastName = member.LastName
                };
                member.OwnedVehicles = vehicles.Where(v => v.MemberId == member.MemberId).ToList();
                model.Count = member.OwnedVehicles.Count;
                models.Add(model);
            }

            return View(models);
        }

        // Get: Members/CheckEmail
        public IActionResult CheckEmail()
        {
            return View();
        }

        // Post: Members/CheckEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckEmail(EmailAddress emailAddress)
        {
            var result = _context.Members.FirstOrDefault(m => m.Email == emailAddress.Email);
            if (result != null)
            {
                TempDataExtensions.Set(TempData, "member", result);
                TempData.Keep();
                return RedirectToAction(nameof(VehiclesController.Park), "Vehicles");
            }
            return RedirectToAction(nameof(RegistrationRequired));
        }

        public IActionResult RegistrationRequired()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var member = _context.Members.Where(m => m.MemberId == id);
            var model = await mapper.ProjectTo<MemberDetailsViewModel>(member).FirstOrDefaultAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()

        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(MemberRegisterViewModel model)
        {
            if (ModelState.IsValid) {
                var member = mapper.Map<Member>(model);
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            var model = mapper.Map<MemberEditViewModel>(member);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
            [Bind("Id,FirstName,LastName,CityAddress,Email,PhoneNumber")] MemberEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberId == model.Id);
                    mapper.Map(model, member);
                    if (member != null) {
                        _context.Update(member);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!_context.Members.Any(m => m.MemberId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id }) ;
            }
            return View(model);
        }

        public IActionResult ValidateEmail(string email)
        {
            if(_context.Members.Any(m => m.Email == email)) 
            {
                return Json($"The email {email} is already in use.");
            }
            return Json(true);
        }
    }
}
