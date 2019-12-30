using System.Collections.Generic;

namespace Garage2.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get => FirstName + " " + LastName; }
        public string CityAddress { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<ParkedVehicle> OwnedVehicles { get; set; }
    }
}
