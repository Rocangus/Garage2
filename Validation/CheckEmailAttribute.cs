using Garage2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.Validation
{
    public class CheckEmailAttribute: ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value is string input)
            {
                var _context = (GarageContext)validationContext.GetService(typeof(GarageContext));
                if (_context.Members.Where(m => m.Email.Equals(input)).Any())
                {
                    return new ValidationResult("Email already in use");
                }
                else
                    return ValidationResult.Success;

            }
            throw new Exception("CheckEmailAttribute did not recieve a string");
        }
    }
}
