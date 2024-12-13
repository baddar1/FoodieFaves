using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FF.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        [JsonIgnore]
        public Restaurant Restaurant { get; set; }
        

        public string ReviewCode { get; set; }
        public bool IsUsed { get; set;}=false;
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User? UserNav { get; set; }
        public int? ReviewId { get; set; }
        [ForeignKey("ReviewId")]
        [ValidateNever]
        public Review? ReviewNav { get; set; }

    }
}
