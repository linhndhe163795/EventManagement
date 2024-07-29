using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.DAO;
using MyAPI.Helper;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDAO _userDAO;
        private readonly Helpers _helpers;
        public UserController(UserDAO userDAO, Helpers helper) 
        {
            _userDAO = userDAO;
            _helpers = helper;
        }
        [HttpGet("userProfile")]
        public ActionResult GetUserProfile() 
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
            var userProfile = _userDAO.getUserProfile(userId);
            if (userProfile == null) 
            {
                return NotFound();
            }
            else
            {
                return Ok(userProfile);
            }
        }
    }
}
