using System;
using System.ComponentModel.DataAnnotations;

namespace Garage2.Models
{
    public class VehicleType : IComparable
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is VehicleType type)
            {
                return Name.CompareTo(type.Name);
            }
            throw new ArgumentException("Object is not a VehicleType");
        }
    }
}