using System.ComponentModel.DataAnnotations;

namespace MyWebClient.DTO.EventDTOs
{
    public class EventTimeDTO
    {
        public int EventId { get; set; }
        [Required(ErrorMessage = "Event Name is required")]
        public string EventName { get; set; } = null!;
        [Required(ErrorMessage = "Event Date is required")]
        public DateTime EventDate { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Number Person is required")]
        public int? NumberPerson { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public string? Location { get; set; }
        [Required(ErrorMessage = "Event Description is required")]
        public string? EventDescription { get; set; }
        public IFormFile? ImageUpload { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }
        public DateTime DateTimeUpdate { get; set; }
    }
}
