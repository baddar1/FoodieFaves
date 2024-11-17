using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models.Dto.ReviewDto
{
    public class ReviewDto
    {
            public double Rating { get; set; }
            public string? Comment { get; set; }
            public string UserId { get; set; }
            public string? UserName { get; set; }
            public int RestaurantId { get; set; }
            public string? RestaurantName { get; set; }
            public int? UserPoints { get; set; }

    }
}
