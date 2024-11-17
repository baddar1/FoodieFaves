using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models.Dto.RestaurantDto
{
    public class RestaurantPointsDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public int Points { get; set; }
    }
}
