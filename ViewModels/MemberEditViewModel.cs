using Garage2.Validation;
using System.ComponentModel.DataAnnotations;

namespace Garage2.ViewModels
{
    public class MemberEditViewModel
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "City Address")]
        public string CityAddress { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
