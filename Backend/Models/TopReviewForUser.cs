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
    public class TopReviewForUser
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public User UserNav { get; set; }

        public int? ReviewId { get; set; }
        [ForeignKey(nameof(ReviewId))]
        [ValidateNever]
        public Review? ReviewNav { get; set; }

        public int RestaurantId { get; set; }
        [ForeignKey(nameof(RestaurantId))]
        [ValidateNever]
        public Restaurant RestaurantNav { get; set; }
        public double TopRate { get; set; }



    }
}
