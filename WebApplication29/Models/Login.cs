using System;
using System.Collections.Generic;

namespace WebApplication29.Models
{
    public partial class Login
    {
        public Login()
        {
            Users = new HashSet<User>();
        }

        public int LoginId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfimPassword { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; }
    }
}
