using FF.Models.Dto.RestaurantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models.Dto.UserDto
{
    public class UserPointsDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<RestaurantPointsDto> RestaurantPoints { get; set; }
    }
}
