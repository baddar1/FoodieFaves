using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models
{
    public class Like
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Review")]
        public int ReviewId { get; set; }
        public DateTime CreatedAt { get; set; }
        // NAV 
        public User User { get; set; }
        public Review Review { get; set; }

    }
}
