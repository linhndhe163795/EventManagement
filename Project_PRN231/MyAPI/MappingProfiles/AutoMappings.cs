using AutoMapper;
using MyAPI.DTOs.EventCategoryDTOs;
using MyAPI.DTOs.EventDTOs;
using MyAPI.DTOs.UserDTOs;
using MyAPI.DTOs.UserEventDTOs;
using MyAPI.Models;

namespace MyAPI.MappingProfiles
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();
            CreateMap<User, UserProfileDTO>().ReverseMap();
            CreateMap<Event, EventDTO>().ReverseMap();
		   
            CreateMap<EventDTO,Event>().ReverseMap();
            CreateMap<EventCategory, CategoryEventDTO>().ReverseMap();
            CreateMap<UserEvent, UserEventDTO>().ReverseMap();
		}
    }
}
