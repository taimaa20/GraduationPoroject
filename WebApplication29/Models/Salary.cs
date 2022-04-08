using System;
using System.Collections.Generic;

namespace WebApplication29.Models
{
    public partial class Salary
    {
        public int SalaryId { get; set; }
        public double Salary1 { get; set; }
        public double? Tracks { get; set; }
        public double? Inventives { get; set; }
        public DateTime MonthOfSalary { get; set; }
        public double TotalSalary { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
