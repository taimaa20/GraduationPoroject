using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} character", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{6,}$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character")]
        public string Password { get; set; } = null!;

        [Display(Name = "ConfimPassword")]
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfimPassword { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; }
    }
}
