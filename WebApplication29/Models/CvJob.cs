using System;
using System.Collections.Generic;

namespace WebApplication29.Models
{
    public partial class CvJob
    {
        public int CvId { get; set; }
        public int UserId { get; set; }
        public int? YearsOfExperiance { get; set; }
        public string? LastWorkPlace { get; set; }
        public string? JobDescription { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
