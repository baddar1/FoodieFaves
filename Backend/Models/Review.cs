using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FF.Models
{
    public class Review
    {
        [Key]
        public int  Id{ get; set; }
        [Required]
        public double Rating { get; set; }
        public string? Comment{ get; set; }
        public int? Likes { get; set; } = 0;
        public int? Points { get; set; } = 0;
        public bool? IsReported { get; set; }
   
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public ICollection<TopReviewForUser> TopReviews { get; set; }

        //Navigation for the Relationship Many To one Brtween Restaurant and Reviwe
        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        [ValidateNever]
        [JsonIgnore]
        public Restaurant RestaurantNav { get; set; }
        //Adding Relationship One To One Between User and Reviwe
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        [JsonIgnore]
        public User UserNav { get; set; }
        //Adding Relationship One To One Between Notification and Reviwe
        public int? NotificationId { get; set; }
        [ForeignKey("NotificationId")]
        [ValidateNever]
        public Notification? NotificationNav { get; set; }

        public string? AdminId { get; set; }
        [ForeignKey("AdminId")]
        [ValidateNever]
        public Admin? AdminNav { get; set; }
        public ICollection<Like>? LikesNav { get; set; }

    }
}
