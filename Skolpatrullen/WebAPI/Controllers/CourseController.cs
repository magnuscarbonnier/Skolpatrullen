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
    public class CourseController : APIController
    {
        public CourseController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpGet]
        [Route("[controller]/GetAll")]
        public APIResponse<IEnumerable<Course>> GetAll()
        {
            APIResponse<IEnumerable<Course>> response = new APIResponse<IEnumerable<Course>>();
            var courselist = _context.Courses.OrderBy(s => s.Name).ToList();
            if (courselist != null)
            {
                response.Data = courselist;
                response.Success = true;
                response.SuccessMessage = "Hämtade alla kurser";
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
                    response.FailureMessage = $"Startdatum kan inte vara senare än slutdatum";
                    response.Success = false;
                }
                else
                {
                    _context.Courses.Add(course);
                    _context.SaveChanges();
                    response.Data = course;
                    response.Success = true;
                    response.SuccessMessage = $"La till kurs med namn {course.Name}";
                }
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/RemoveCourse/{id}")]
        public APIResponse Remove(int id)
        {
            APIResponse response = new APIResponse();
            var removecourse = _context.Courses.SingleOrDefault(s => s.Id == id);
            if (removecourse != null)
            {
                _context.Remove(removecourse);
                _context.SaveChanges();
                response.Success = true;
                response.SuccessMessage = $"Tog bort kurs med id {id} och namn {removecourse.Name}";
            }
            else
            {
                response.FailureMessage = $"Kursen fanns inte";
                response.Success = false;
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
            response.SuccessMessage = $"Hämtadde kurs med id {Id}";
            return response;
        }
    }
}