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

        public string Email { get; set; }

        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassord { get; set; }
        public string UserName { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }


    }
}
