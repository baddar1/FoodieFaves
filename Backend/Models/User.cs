
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
        public string? ImgUrl { get; set; }
        public int TotalLikes { get; set; } = 0;
        public int? ReviewCount { get; set; } = 0;
        public int? TotalPoints { get; set; } = 0;
        public double? TopRateReview { get; set; }
        public ICollection<TopReviewForUser>? TopReviews { get; set; }
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
