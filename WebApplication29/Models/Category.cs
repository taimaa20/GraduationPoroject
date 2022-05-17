using System;
using System.Collections.Generic;

namespace WebApplication29.Models
{
    public partial class Category
    {
        public Category()
        {
            Services = new HashSet<Service>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Service> Services { get; set; }
    }
}
