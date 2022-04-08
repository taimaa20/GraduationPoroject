using System;
using System.Collections.Generic;

namespace WebApplication29.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; } = null!;
        public string? SenderName { get; set; }
        public string? MessageTitle { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
