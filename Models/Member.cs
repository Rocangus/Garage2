using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CityAddress { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<ParkedVehicle> OwnedVehicles { get; set; }
    }
}
