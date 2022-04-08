using System;
using System.Collections.Generic;

namespace WebApplication29.Models
{
    public partial class ContactU
    {
        public int ContactId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MessageText { get; set; } = null!;
    }
}
