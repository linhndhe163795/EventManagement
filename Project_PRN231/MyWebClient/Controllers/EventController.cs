using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebClient.DTO.EventDTOs;
using MyWebClient.DTO.EventCategoryDTOs;
using MyWebClient.Helper;
using MyWebClient.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MyWebClient.Controllers
{
    public class EventController : Controller
    {
        private string eventApiUrl = "";
        private string eventCategory = "";
        private readonly HttpClient client = null;
        private readonly ImageHelper _imageHelper;
        public EventController(ImageHelper imageHelper)
        {
            _imageHelper = imageHelper;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            eventApiUrl = "http://localhost:5022/api/Events";
            eventCategory = "http://localhost:5022/api/Category";
        }
        [HttpGet]
        public async Task<IActionResult> ListEvent(string searchEvent)
        {
            SetAuthorizationHeader();
            HttpResponseMessage res_2 = await client.GetAsync(eventApiUrl + "/updateRealTime");
            if (searchEvent != null)
            {
                HttpResponseMessage res_1 = await client.GetAsync(eventApiUrl + "/getByName/" + searchEvent);
                if (res_1.IsSuccessStatusCode && res_2.IsSuccessStatusCode)
                {

                    string content = await res_1.Content.ReadAsStringAsync();
                    List<EventDTO> listEvent = JsonConvert.DeserializeObject<List<EventDTO>>(content);
                    ViewData["events"] = listEvent;
                    ViewData["search"] = searchEvent;
                    return View();
                }
                else
                {
                    TempData["notification"] = "You need login to system !";
                    return RedirectToAction("Login", "User");
                }
            }
            HttpResponseMessage res = await client.GetAsync(eventApiUrl);
            if (res.IsSuccessStatusCode && res_2.IsSuccessStatusCode)
            {

                string content = await res.Content.ReadAsStringAsync();
                List<EventDTO> listEvent = JsonConvert.DeserializeObject<List<EventDTO>>(content);
                ViewData["events"] = listEvent;
                return View();
            }
            else
            {
                TempData["notification"] = "You need login to system !";
                return RedirectToAction("Login", "User");
            }
        }
        [HttpGet]
        public async Task<IActionResult> EventDetails(int id)
        {
            SetAuthorizationHeader();
            HttpResponseMessage res_1 = await client.GetAsync(eventApiUrl + "/id?id=" + id);
            HttpResponseMessage res_2 = await client.GetAsync(eventCategory);
            if (res_1.IsSuccessStatusCode && res_2.IsSuccessStatusCode)
            {

                string content_1 = await res_1.Content.ReadAsStringAsync();
                var eventById = JsonConvert.DeserializeObject<EventDTO>(content_1);
                string content_2 = await res_2.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<List<CategoryEventDTO>>(content_2);
                ViewData["eventById"] = eventById;
                ViewData["category"] = category;
                return View(eventById);
            }
            else
            {
                TempData["notification"] = "Not found any event";
                return RedirectToAction("ListEvent", "Event");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEvent(int id, EventDTO e)
        {
            SetAuthorizationHeader();

            if (e.ImageUpload != null)
            {
                e.Image = await _imageHelper.UploadImageAsync(e.ImageUpload);
            }
            else
            {
                e.Image = e.Image;
            }

            HttpResponseMessage res = await client.PutAsJsonAsync(eventApiUrl + "/id?id=" + id, e);
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("ListEvent", "Event");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> DeleteEvent(int id)
        {
            SetAuthorizationHeader();
            HttpResponseMessage res = await client.DeleteAsync(eventApiUrl + "/id?id=" + id);
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("ListEvent", "Event");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddEvent()
        {
            SetAuthorizationHeader();
            HttpResponseMessage res_2 = await client.GetAsync(eventCategory);
            HttpResponseMessage res = await client.GetAsync(eventApiUrl);
            if (res_2.IsSuccessStatusCode && res.IsSuccessStatusCode)
            {

                string content_2 = await res_2.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<List<CategoryEventDTO>>(content_2);
                ViewData["category"] = category;
                return View();
            }
            else
            {
                TempData["notification"] = "You need login to system !";
                return RedirectToAction("Login", "User");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddEvent(EventDTO eventDTO)
        {
            SetAuthorizationHeader();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddEvent", "Event");
            }

            if (eventDTO.ImageUpload != null)
            {
                eventDTO.Image = await _imageHelper.UploadImageAsync(eventDTO.ImageUpload);
            }
            HttpResponseMessage res = await client.PostAsJsonAsync(eventApiUrl + "/addEvent", eventDTO);
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("ListEvent", "Event");
            }
            else
            {
                TempData["notification"] = "You need login to system !";
                return RedirectToAction("Login", "User");
            }
        }
        [HttpGet]
        public async Task<IActionResult> History(string searchEvent)
        {
            bool checkAuthen = SetAuthorizationHeader();
            HttpResponseMessage res = await client.GetAsync(eventApiUrl + "/historyEvent");
            HttpResponseMessage res_2 = await client.GetAsync(eventApiUrl + "/searchHistory/" + searchEvent);

            if (!checkAuthen)
            {
                TempData["notification"] = "You need login to system !";
                return RedirectToAction("Login", "User");
            }
            if (res.IsSuccessStatusCode && checkAuthen && searchEvent == null)
            {
                string content_1 = await res.Content.ReadAsStringAsync();
                var historyEvent = JsonConvert.DeserializeObject<List<EventDTO>>(content_1);
                ViewData["historyEvent"] = historyEvent;
                return View();
            }
            if(searchEvent != null && checkAuthen && res_2.IsSuccessStatusCode)
            {
                string content_1 = await res_2.Content.ReadAsStringAsync();
                var historyEvent = JsonConvert.DeserializeObject<List<EventDTO>>(content_1);
                ViewData["historyEvent"] = historyEvent;
                ViewData["seachEvent"] = searchEvent;
                return View();
            }
            else
            {
                Console.WriteLine("Status: " + res.StatusCode);
                Console.WriteLine("Boolean: " + res.StatusCode.Equals("Unauthorized"));
                Console.WriteLine("IsSuccessStatusCode: " + res.IsSuccessStatusCode);
                ViewData["messageEvent"] = "Not Found Event";
                return View();
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
