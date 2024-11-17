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
        public int ReviewId { get; set; }
        [ForeignKey("ReviewId")]
        [ValidateNever]
        public Review ReviewNav { get; set; }
        //Adding Relationship One To Many Between User and Notification
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User UserNav { get; set; }
        Restaurant FavRestaurant { get; set; }
        User FavBlogger { get; set; }
     
    }
}
