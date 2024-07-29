using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPI.DAO;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly EventCategoryDAO _eventCategoryDAO;
        public CategoryController(EventCategoryDAO eventCategoryDAO)
        {
            _eventCategoryDAO = eventCategoryDAO;
        }
        [HttpGet]
        public IActionResult getCategory()
        {
            var category = _eventCategoryDAO.getCategoryEvent();
            return Ok(category);
        }

    }
}
