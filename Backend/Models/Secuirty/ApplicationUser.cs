using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models.Secuirty
{
    public class ApplicationUser : IdentityUser
    {

        public virtual User UserDetails { get; set; }

    }
}
