using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Models
{
    public class PendingUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }
}
