using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models
{
    public class Admin
    {
        [Key]
        public string Id { get; set; }
        //Add Relationshipe Between Reviwe,Users,Restaurant and Admin.
        public ICollection< User> ManageUsers { get; set; }
        public ICollection<Restaurant> ManageRestaurants { get; set; }
        public ICollection<Review>ManageReviews { get; set; }

    }
}
