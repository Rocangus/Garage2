using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.ViewModels
{
    public class MemberViewModel
    {
        [Display (Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "City Address")]
        public string CityAddress { get; set; }
        [Display(Name = "Email Address")]
        [Remote(action: "ValidateEmail", controller: "Members")]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
