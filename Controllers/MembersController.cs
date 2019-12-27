using Garage2.Data;
using Garage2.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

            var model = new MemberSummaryViewModel();

            

            return View();
        }
    }
}
