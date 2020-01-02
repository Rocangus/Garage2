using Garage2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.ViewModels
{
    public class MemberDetailsViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName { get => FirstName + " " + LastName; }

        [Display(Name = "City Address")]
        public string CityAddress { get; set; }
        [Display(Name = "Email Address")]
        [Remote(action: "ValidateEmail", controller: "Members")]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public ICollection<ParkedVehicle> OwnedVehicles { get; set; }
    }
}
