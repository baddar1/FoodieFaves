using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models.Dto.UserDto
{
    public class UpdateUserName 
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Username must be between 3 and 20 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers.")]
        public string UserName { get; set; }
    }
}
