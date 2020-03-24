using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class CourseBlogController : APIController
    {
        public CourseBlogController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }
        [HttpGet]
        [Route("[controller]/GetBlogPostsByCourseId/{id}")]
        public APIResponse<IEnumerable<CourseBlogPost>> GetBlogPostsByCourseId(int id)
        {
            APIResponse<IEnumerable<CourseBlogPost>> response = new APIResponse<IEnumerable<CourseBlogPost>>();
 
            var courseBlogList = from cbp in _context.CourseBlogPosts
                           join co in _context.Courses on cbp.CourseId equals co.Id
                           join us in _context.Users on cbp.UserId equals us.Id
                           where cbp.CourseId == id
                           orderby cbp.PublishDate descending
                           select new CourseBlogPost
                           {
                               Id = cbp.Id,
                               Title=cbp.Title,
                               Content=cbp.Content,
                               HashTags=cbp.HashTags,
                               PublishDate=cbp.PublishDate,
                               Course=co,
                               CourseId=cbp.CourseId,
                               UserId = cbp.UserId,
                               User = us
                           };
            if (courseBlogList != null)
            {
                response.Data = courseBlogList;
                response.Success = true;
                response.SuccessMessage = $"Hämtade alla inlägg för kurs med id: {id}";
            }
            return response;
        }
        //[HttpPost]
        //[Route("[controller]/Add")]
        //public APIResponse<Course> Add(Course course)
        //{
        //    APIResponse<Course> response = new APIResponse<Course>();
        //    if (course != null)
        //    {
        //        if (course.StartDate > course.EndDate)
        //        {
        //            response.FailureMessage = $"Startdatum kan inte vara senare än slutdatum";
        //            response.Success = false;
        //        }
        //        else
        //        {
        //            _context.Courses.Add(course);
        //            _context.SaveChanges();
        //            response.Data = course;
        //            response.Success = true;
        //            response.SuccessMessage = $"La till kurs med namn {course.Name}";
        //        }
        //    }
        //    return response;
        //}
    }
}
