using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models.Dto.UserDto
{
    public class UserInfoDto
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? UnReadNotiNum { get; set; } = 0;
        public int? UserPoints { get; set; }

    }
}
