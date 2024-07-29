using System;
using System.Collections.Generic;

namespace MyAPI.Models
{
    public partial class Event
    {
        public Event()
        {
            UserEvents = new HashSet<UserEvent>();
        }

        public int EventId { get; set; }
        public string EventName { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public int? CategoryId { get; set; }
        public int? NumberPerson { get; set; }
        public string? Location { get; set; }
        public string? EventDescription { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }
        public virtual EventCategory? Category { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }
    }
}
