using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MyAPI.DTOs.UserDTOs;
using MyAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyAPI.DAO
{
    public class UserDAO
    {
        private readonly EventManagementContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserDAO(EventManagementContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public void register(RegisterDTO register)
        {
            var data = _mapper.Map<User>(register);
            _context.Add(data);
            _context.SaveChanges();
        }
        public bool checkAccountExsit(RegisterDTO register)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username.Equals(register.Username) || x.Email.Equals(register.Email));
            if (user != null)
            {
                return true;
            }
            return false;
        }
        public async Task<LoginDTO> checkLogin(string? userName, string password)
        {

            User userLogin = _context.Users.FirstOrDefault(x => (x.Username.Equals(userName) || x.Email.Equals(userName))
                                                            && x.Password.Equals(password));
            if(userLogin != null)
            {
                var dataMapper = _mapper.Map<LoginDTO>(userLogin);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                      new Claim(ClaimTypes.Email, dataMapper.Email.ToString()),
                      new Claim(ClaimTypes.Name, dataMapper.Username.ToString()),
                      new Claim("ID", userLogin.UserId.ToString()),
                    }),
                    IssuedAt = DateTime.UtcNow,
                    Issuer = _configuration["JWT:Issuer"],
                    Audience = _configuration["JWT:Audience"],
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                dataMapper.Token = tokenHandler.WriteToken(token);

                return dataMapper;
            }
            return null;
        }

        public UserProfileDTO getUserProfile(int userId)
        {
            var userProfile = _context.Users.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
            var userMapper = _mapper.Map<UserProfileDTO>(userProfile);

            return userMapper;
        }

    }
}
