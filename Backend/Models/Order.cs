using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public Restaurant Restaurant { get; set; }
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        public string ReviewCode { get; set; }

    }
}
