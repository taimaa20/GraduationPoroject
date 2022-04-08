using System;
using System.Collections.Generic;

namespace WebApplication29.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public double? ReviewRange { get; set; }
        public string ReviewText { get; set; } = null!;
        public int UserServiceId { get; set; }

        public virtual UserService UserService { get; set; } = null!;
    }
}
