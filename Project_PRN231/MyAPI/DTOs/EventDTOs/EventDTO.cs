namespace MyAPI.DTOs.EventDTOs
{
	public class EventDTO
	{
		public int EventId { get; set; }

		public string EventName { get; set; } = null!;
		public DateTime EventDate { get; set; }
		public int? CategoryId { get; set; }
		public string? CategoryName { get; set; }
		public int? NumberPerson { get; set; }
		public string? Location { get; set; }
		public string? EventDescription { get; set; }
		public string? Status { get; set; }
        public string? Image { get; set; }
    }
}
