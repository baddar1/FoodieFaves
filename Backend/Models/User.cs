
using FF.Models.Secuirty;
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
    public class User
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [DisplayName("User Name")]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ICollection<Points> UserRestaurantPoints { get; set; }

        [DisplayName("Phone Number")]
        [MaxLength(10)]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        //Adding rerlationship Between Notification and User
        public ICollection<Notification>? Notifications { get; set; }
        //Adding rerlationship Between Restaurant and User
        public ICollection<FavoriteRestaurants>? FavoriteRestaurants { get; set; } = new List<FavoriteRestaurants>();
        public ICollection<FavoriteBlogger>? FavoriteBloggers { get; set; } = new List<FavoriteBlogger>();
        //Adding Relationship in the User
        //public string? UserId { get; set; }
        //[ForeignKey("UserId")]
        //[ValidateNever]
        //public User? UserNav { get; set; }
        //Navigation for the User Nav
        //public ICollection<User>? FavoriteBloggers { get; set; }
        //Adding rerlationship Between Reviwe and User
        public ICollection<Review>? Reviews { get; set; }

       // Foreign key to ApplicationUser
        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; } // Navigation property

        public string? AdminId { get; set; }
        [ForeignKey("AdminId")]
        [ValidateNever]
        public Admin? AdminNav { get; set; }
        public ICollection<Like>? Likes { get; set; } 
    }
}
