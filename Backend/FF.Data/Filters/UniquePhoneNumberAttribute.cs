using FF.Data.Access.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models.Filters
{
    internal class UniquePhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value , ValidationContext validationContext)
        {
            var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var phoneNumber = value as string;
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return ValidationResult.Success;
            }
            var exists = dbContext.Users.Any(u => u.PhoneNumber == phoneNumber);
            if (exists) 
            {
                return new ValidationResult( "The phone number is already in use.");
            }
            return ValidationResult.Success;
            
        }
    }
}
