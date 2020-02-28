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
        [HttpGet]
        [Route("[controller]/GetAll")]
        public APIResponse<IEnumerable<Course>> GetAll()
        {
            APIResponse<IEnumerable<Course>> response = new APIResponse<IEnumerable<Course>>();
            var courselist = _context.Courses.OrderBy(s => s.Name).ToList();
            if (courselist != null)
            {
                response.Success = true;
                response.Data = courselist;
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public APIResponse<Course> Add(Course course)
        {
            APIResponse<Course> response = new APIResponse<Course>();
            if (course != null)
            {
                if (course.StartDate > course.EndDate)
                {
                    response.ErrorMessages[0] = "Startdatum kan inte vara senare än slutdatum";
                    response.Success = false;
                }
                else
                {
                    _context.Courses.Add(course);
                    _context.SaveChanges();
                    response.Success = true;
                    response.Data = course;
                }
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/RemoveCourse/{id}")]
        public APIResponse<bool> Remove(int id)
        {
            APIResponse<bool> response = new APIResponse<bool>();
            var removecourse = _context.Courses.SingleOrDefault(s => s.Id == id);
            if (removecourse != null)
            {
                _context.Remove(removecourse);
                _context.SaveChanges();
                response.Success = true;
            }
            else
            {
                response.ErrorMessages.Add($"Kursen fanns inte");
                response.Success = false;
            }
            return response;
        }
    }
}