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
        [HttpGet]
        [Route("[controller]/GetAllCourses")]
        public APIResponse<IEnumerable<Course>> GetAllCourses()
        {
            APIResponse<IEnumerable<Course>> response = new APIResponse<IEnumerable<Course>>();
            var courseList = _context.Courses.OrderBy(s => s.Name).ToList();
            if (courseList != null)
            {
                response.Success = true;
                response.Data = courseList;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetCourseById/{Id}")]
        public APIResponse<Course> GetCourseById(int Id)
        {
            APIResponse<Course> response = new APIResponse<Course>();
            response.Data = _context.Courses.SingleOrDefault(c => c.Id == Id);

            response.Success = true;
            return response;
        }
    }
}