using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication29.Models
{
    public partial class User
    {
        public User()
        {
            CvJobs = new HashSet<CvJob>();
            Messages = new HashSet<Message>();
            Salaries = new HashSet<Salary>();
            Services = new HashSet<Service>();
            UserServices = new HashSet<UserService>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Adress { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public int LoginId { get; set; }
        public string? UserImage { get; set; }
        [NotMapped] // database dont have a column
        [DisplayName("Upload File")] // to show in view
        public IFormFile? ImageFile { get; set; } // data type name IFormFile

        public virtual Login Login { get; set; } = null!;
        public virtual ICollection<CvJob> CvJobs { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<UserService> UserServices { get; set; }
    }
}
