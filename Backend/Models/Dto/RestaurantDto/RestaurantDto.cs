using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using FF.Models.Dto.ReviewDto;

namespace FF.Models.Dto.RestaurantDto
{
    public class RestaurantDto
    {
        [Required]
        [DisplayName("Restaurnat Number")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        [MaxLength(10)]
        public string phoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Cuisine { get; set; }
        [Required]
        public double Budget { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        public string? LiveSite { get; set; }
        public string? Open { get; set; }
        public string? Close { get; set; }
        public string? Description { get; set; }
    }
}
