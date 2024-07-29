namespace MyAPI.DTOs.UserDTOs
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public string? Avatar { get; set; }
    }
}
