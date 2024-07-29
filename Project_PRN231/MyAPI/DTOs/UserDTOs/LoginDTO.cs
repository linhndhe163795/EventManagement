namespace MyAPI.DTOs.UserDTOs
{
    public class LoginDTO
    {
        public string? Username { get; set; }
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string? Token { get; set; } = "";
    }
}
