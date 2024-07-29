using Microsoft.AspNetCore.Mvc;
using MyWebClient.DTO.User;
using MyWebClient.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MyWebClient.Controllers
{
    public class UserController : Controller
    {
		private string authApiUrl = "";
		private string userApiUrl = "";
		private readonly HttpClient client = null;
		public UserController(ILogger<UserController> logger)
        {
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
            authApiUrl = "http://localhost:5022/api/Auth";
			userApiUrl = "http://localhost:5022/api/User";


        }
        [HttpGet]
		public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
        }
        [HttpPost]
		public async Task<IActionResult> Login(UserLoginDTO user)
		{
			HttpResponseMessage res = await client.PostAsJsonAsync(authApiUrl + "/login", user);

			if (res.IsSuccessStatusCode)
			{
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				string content = await res.Content.ReadAsStringAsync();
				UserLoginDTO userDTO = JsonConvert.DeserializeObject<UserLoginDTO>(content);
				string? token = userDTO.token;
				HttpContext.Session.SetString("JwtToken", token);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ViewData["Message"] = "Incorrect Email or Password";
				return View(nameof(Login));
			}
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterDTO user)
		{
			HttpResponseMessage res = await client.PostAsJsonAsync(authApiUrl + "/register", user);
			if (res.IsSuccessStatusCode)
			{
				return RedirectToAction("Login", "User");
			}
			else
			{
                ViewData["Message"] = "Email or User Name is exsit! Input again";
                return View(nameof(Register));
			}
		}
		[HttpGet]
		public async Task<IActionResult> UserProfile()
		{
            bool checkAuthen = SetAuthorizationHeader();
            HttpResponseMessage res = await client.GetAsync(userApiUrl + "/userProfile");
			if (!checkAuthen)
			{
                TempData["notification"] = "You need login to system !";
                return RedirectToAction("Login", "User");
            }
			if (res.IsSuccessStatusCode && checkAuthen) 
			{
				string content = await res.Content.ReadAsStringAsync();
				UserProfileDTO user = JsonConvert.DeserializeObject<UserProfileDTO>(content);
				ViewData["userProfile"] = user;
				return View(user);
			}
			else
			{
				 TempData["notification"] = "You need login to system !";
                return RedirectToAction("Login", "User");
			}
			
		}
        private bool SetAuthorizationHeader()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"Token: {token}");
                return true;
            }
            else
            {
                Console.WriteLine("Token is null or empty");
                return false;
            }
        }
    }
}
