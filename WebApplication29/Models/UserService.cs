using System;
using System.Collections.Generic;

namespace WebApplication29.Models
{
    public partial class UserService
    {
        public UserService()
        {
            Reviews = new HashSet<Review>();
        }

        public int UserServiceId { get; set; }
        public int ServiceId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = null!;
        public int UserId { get; set; }

        public virtual Service Service { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
