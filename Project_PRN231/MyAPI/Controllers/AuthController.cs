using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.DAO;
using MyAPI.DTOs.UserDTOs;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserDAO _userDAO;
        private readonly IMapper _mapper;
        private IConfiguration _configuration;
        public AuthController(IMapper mapper, IConfiguration configuration, UserDAO userDAO)
        {
            _mapper = mapper;
            _userDAO = userDAO;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult registerUser(RegisterDTO register)
        {
            if (register == null)
            {
                return BadRequest();
            }
            bool checkAccountExist = _userDAO.checkAccountExsit(register);
            if (checkAccountExist)
            {
                return BadRequest("Account Exist");
            }
            _userDAO.register(register);
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDTO login)
        {
            if (login == null)
            {
                return BadRequest("input information not null");
            }
            LoginDTO outLogin = await _userDAO.checkLogin(login.Username, login.Password);

            if (outLogin != null)
            {
               
                return Ok(outLogin);
            }
            return BadRequest("Login unsuccessfull");
        }
       
    }
}
