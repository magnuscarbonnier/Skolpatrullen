using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILogger<CourseController> _logger;
        public CourseController(Context context, ILogger<CourseController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public APIResponse<Course> Add(Course course)
        {
            APIResponse<Course> response = new APIResponse<Course>();
            if (course != null)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                response.Success = true;
                response.Data = course;
            }
            return response;
        }
    }
}