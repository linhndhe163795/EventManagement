using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.DAO;
using MyAPI.Models;
using MyAPI.Helper;
using System.IdentityModel.Tokens.Jwt;
using MyAPI.DTOs.EventDTOs;
using MyAPI.DTOs.UserEventDTOs;
using Newtonsoft.Json.Linq;

namespace MyAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventDAO _eventDAO;
        private readonly EventCategoryDAO _eventCategoryDAO;
        private readonly UserEventDAO _userEventDAO;
        private readonly IMapper _mapper;
        private IConfiguration _configuration;
        private readonly Helpers _helpers;

        public EventsController(IMapper mapper, IConfiguration configuration, EventDAO eventDAO, Helpers helpers, UserEventDAO userEventDAO)
        {
            _mapper = mapper;
            _eventDAO = eventDAO;
            _configuration = configuration;
            _helpers = helpers;
            _userEventDAO = userEventDAO;
        }
        [HttpGet]
        public IActionResult listEvent()
        {

            string token = Request.Headers["Authorization"];
            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }
            int userId = _helpers.GetIdInHeader(token);
            var listEvent = _eventDAO.getListEvent(userId);
            return Ok(listEvent);
        }
        [HttpPost("addEvent")]
        public async Task<IActionResult> addEvent(EventDTO events)
        {
            string token = Request.Headers["Authorization"];
            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }
            int userId = _helpers.GetIdInHeader(token);

            await _eventDAO.addEvent(events);
            int lastID = _eventDAO.getLastID();
            UserEventDTO ue = new UserEventDTO
            {
                UserId = userId,
                EventId = lastID,
                Status = "True"
            };
            _userEventDAO.Add(ue);
            return Ok();
        }
        [HttpPut("id")]
        public IActionResult updateEvent(int id, EventDTO events)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool checkNameEvent = _eventDAO.checkEventExist(id);
            if (!checkNameEvent)
            {
                return BadRequest("Event is exsit in database");
            }
            _eventDAO.updateEventById(id, events);
            return Ok("update success");
        }
        [HttpDelete("id")]
        public IActionResult deleteEvent(int id)
        {
            bool checkNameEvent = _eventDAO.checkEventExist(id);
            if (!checkNameEvent)
            {
                return BadRequest("Event is exsit in database");
            }
            var events = _eventDAO.GetEvent(id);
            _eventDAO.delete(events);
            return Ok("delete success");
        }
        [HttpGet("id")]
        public IActionResult getEventById(int id)
        {
            var eventById = _eventDAO.getEventById(id);

            return Ok(eventById);
        }
        [HttpGet("getByName/{name}")]
        public IActionResult getEventByName(string name)
        {
            string token = Request.Headers["Authorization"];
            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }
            int userId = _helpers.GetIdInHeader(token);

            var events = _eventDAO.getEventByName(name, userId);

            if (events == null)
            {
                return NotFound("Not Found event name");
            }
            else
            {
                return Ok(events);
            }
        }
        [HttpGet("getEventToDay")]
        public IActionResult getEventByDate()
        {
            string token = Request.Headers["Authorization"];
            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }
            int userId = _helpers.GetIdInHeader(token);
            var events = _eventDAO.getEventByDate(userId);
            if(events == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(events);
            }
        }
        [HttpGet("historyEvent")]
        public IActionResult getListHistoryEvent()
        {
            string token = Request.Headers["Authorization"];
            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }
            int userId = _helpers.GetIdInHeader(token);
            string name = "";
            var events = _eventDAO.getHistoryEvent(userId,name);
            if (events != null) 
            {
                return Ok(events);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpGet("updateRealTime")]
        public IActionResult updateEvent()
        {
            string token = Request.Headers["Authorization"];
            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }
            int userId = _helpers.GetIdInHeader(token);
            _eventDAO.UpdateRealTime();
            return Ok();

        }
        [HttpGet("searchHistory/{name}")]
        public IActionResult searchEventHistory(string name)
        {
            string token = Request.Headers["Authorization"];
            if (token.StartsWith("Bearer"))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }
            int userId = _helpers.GetIdInHeader(token);

            var events = _eventDAO.getHistoryEvent(userId,name);
            return Ok(events);
        }
    }
}