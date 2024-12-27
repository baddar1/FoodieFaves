using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models.Secuirty
{
    public class RegisterModel1
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [StringLength(100, MinimumLength = 8, ErrorMessage = "The password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!#%*?&])[A-Za-z\d#@$!%*?&]{8,}$",
        ErrorMessage = "The password must have at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassord { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Username must be between 3 and 20 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers.")]
        public string UserName { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
    }
}
