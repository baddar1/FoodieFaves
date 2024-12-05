using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models
{
    
    public class FavoriteBlogger
    {
        
        public string UserId { get; set; }
        public User User { get; set; }
        
        public string BloggerId { get; set; }
        public User Blogger { get; set; }

    }
}
