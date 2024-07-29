using System;
using System.Collections.Generic;

namespace MyAPI.Models
{
    public partial class UserEvent
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string? Status { get; set; }

        public virtual Event Event { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
