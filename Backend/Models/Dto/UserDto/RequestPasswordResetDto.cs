using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models.Dto.UserDto
{
    public class RequestPasswordResetDto
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
