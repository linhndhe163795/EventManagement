using AutoMapper;
using MyAPI.DTOs.EventCategoryDTOs;
using MyAPI.Models;

namespace MyAPI.DAO
{
    public class EventCategoryDAO
    {
        private readonly EventManagementContext _context;
        private readonly IMapper _mapper;
        public EventCategoryDAO(EventManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<CategoryEventDTO> getCategoryEvent()
        {
            var listCategory = _context.EventCategories.ToList();
            var categoryMap = _mapper.Map<List<CategoryEventDTO>>(listCategory);
            return categoryMap;

        }
    }
}
