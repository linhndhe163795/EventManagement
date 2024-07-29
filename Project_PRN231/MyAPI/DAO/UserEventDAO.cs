using AutoMapper;
using MyAPI.DTOs.UserEventDTOs;
using MyAPI.Models;

namespace MyAPI.DAO
{
    public class UserEventDAO
    {
        private readonly EventManagementContext _context;
        private readonly IMapper _mapper;
        public UserEventDAO(EventManagementContext context, IMapper mapper)
        {
            _mapper = mapper;   
            _context = context;
        }
        public void Add(UserEventDTO userEvent) 
        {
            var userEventMapper = _mapper.Map<UserEvent>(userEvent);
            _context.Add(userEventMapper);
            _context.SaveChanges();
        }
    }
}
