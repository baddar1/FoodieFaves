using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace FF.Models.Dto.RestaurantDto
{
    public class UpdateRestaurantDto
    {
        [DisplayName("Restaurnat Number")]
        public string Name { get; set; }
        [DisplayName("Phone Number")]
        [MaxLength(10)]
        public string phoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Location { get; set; }
        public string Cuisine { get; set; }
        public double Budget { get; set; }
        public string ImgUrl { get; set; }
        public string? LiveSite { get; set; }
        public string? Open { get; set; }
        public string? Close { get; set; }
        public string? Description { get; set; }
    }
}

