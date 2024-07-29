using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebClient.Models
{
    public partial class Event
    {
        public Event()
        {
            UserEvents = new HashSet<UserEvent>();
        }

        public int EventId { get; set; }
		[Required(ErrorMessage = "Event Name is required")]
		public string EventName { get; set; } = null!;
		[Required(ErrorMessage = "Event Date is required")]
		public DateTime EventDate { get; set; }
        [Required]
        public int? CategoryId { get; set; }
		[Required(ErrorMessage = "Number Person is required")]
		public int? NumberPerson { get; set; }
		[Required(ErrorMessage = "Location is required")]
		public string? Location { get; set; }
		[Required(ErrorMessage = "Event Description is required")]
		public string? EventDescription { get; set; }
        public string? Status { get; set; }
		[Required(ErrorMessage = "Image is required")]
		public string? Image { get; set; }
        public virtual EventCategory? Category { get; set; }
        public virtual ICollection<UserEvent> UserEvents { get; set; }
    }
}
