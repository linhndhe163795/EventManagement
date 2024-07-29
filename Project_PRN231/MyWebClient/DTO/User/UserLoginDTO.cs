namespace MyWebClient.DTO.User
{
	public class UserLoginDTO
	{
		public string Username { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string? token { get; set; } = "";
	}
}
