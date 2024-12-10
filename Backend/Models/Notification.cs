using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        //Adding Relationship One To One Between Reviwe and Notification
        public int? ReviewId { get; set; }
        [ForeignKey("ReviewId")]
        [ValidateNever]
        public Review? ReviewNav { get; set; }
        //Adding Relationship One To Many Between User and Notification
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User UserNav { get; set; }
        public int? RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        [ValidateNever]
        Restaurant FavRestaurant { get; set; }
        public string? BloggertId { get; set; }
        [ForeignKey("BloggerId")]
        [ValidateNever]
        User FavBlogger { get; set; }
        public string NotificationType { get; set; }
        public string Message { get; set; } // Add message to store notification content
        public DateTime CreatedAt { get; set; } // When the notification was created
        public bool IsRead { get; set; } // To mark if the user has read the notification

    }
}
