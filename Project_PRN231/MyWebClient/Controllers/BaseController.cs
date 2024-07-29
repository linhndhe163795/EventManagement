using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MyWebClient.Controllers
{
	public class BaseController : Controller
	{
		protected readonly HttpClient client;

		public BaseController()
		{
			client = new HttpClient();
		}
		protected void SetAuthorizationHeader()
		{
			var token = HttpContext.Session.GetString("JwtToken");
			if (!string.IsNullOrEmpty(token))
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}
		}
	}
}
