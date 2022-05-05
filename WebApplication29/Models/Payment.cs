using System;
using System.Collections.Generic;

namespace WebApplication29.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; } = null!;
        public double? PaymentAmount { get; set; }
        public string CardNumber { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public int? UserServiceId { get; set; }

        public virtual UserService? UserService { get; set; }
    }
}
