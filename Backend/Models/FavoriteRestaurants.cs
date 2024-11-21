using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models
{
    public class FavoriteRestaurants
    {
        
        public string UserId { get; set; }
        public User User { get; set; }
        
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
