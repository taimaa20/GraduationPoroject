using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication29.Models
{
    public partial class Service
    {
        public Service()
        {
            UserServices = new HashSet<UserService>();
        }

        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = null!;
        public string? UserImage { get; set; }
        [NotMapped] // database dont have a column
        [DisplayName("Upload File")] // to show in view
        public IFormFile? ImageFile { get; set; } // data type name IFormFile

        public virtual Category Category { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<UserService> UserServices { get; set; }
    }
}
