using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Restaurnat Number")]
        public string Name { get; set; }
        public double Rating { get; set; }
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
        public string? Description { get; set; }
        public ICollection<FavoriteRestaurants>? FavoriteRestaurants { get; set; }
        public ICollection<FavoriteBlogger> FavoriteBloggers { get; set; }
        public ICollection<Points> UserRestaurantPoints { get; set; }
        [ValidateNever]
        public ICollection<Review> ReviweNav { get; set; }
        public string? AdminId { get; set; }
        [ForeignKey("AdminId")]
        [ValidateNever]
        public Admin? AdminNav { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
