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
        [Route("[controller]/GetBlogPostsByCourseId/{CourseId}")]
        public APIResponse<IEnumerable<CourseBlogPost>> GetBlogPostsByCourseId(int CourseId)
        {
            APIResponse<IEnumerable<CourseBlogPost>> response = new APIResponse<IEnumerable<CourseBlogPost>>();

            var courseBlogList = _context.CourseBlogPosts
                .Include(cb => cb.User)
                .Where(cb => cb.CourseId == CourseId);
            if (courseBlogList != null)
            {
                response.Data = courseBlogList;
                response.Success = true;
                response.SuccessMessage = $"Hämtade alla inlägg för kurs med id: {CourseId}";
            }
            else
            {
                response.Success = false;
                response.FailureMessage = "Kunde inte hämta kursinlägg";
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public APIResponse<CourseBlogPost> Add(CourseBlogPost blogPost)
        {
            APIResponse<CourseBlogPost> response = new APIResponse<CourseBlogPost>();
            if (blogPost != null)
            {
                _context.CourseBlogPosts.Add(blogPost);
                _context.SaveChanges();
                response.Data = blogPost;
                response.Success = true;
                response.SuccessMessage = $"La till inlägg med titel {blogPost.Title}";
            }
            else
            {
                response.Success = false;
                response.FailureMessage = "Fick inget inlägg";
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/Remove/{id}")]
        public APIResponse Remove(int id)
        {
            APIResponse response = new APIResponse();
            var removeBlogPost = _context.CourseBlogPosts.SingleOrDefault(s => s.Id == id);
            if (removeBlogPost != null)
            {
                _context.Remove(removeBlogPost);
                _context.SaveChanges();
                response.Success = true;
                response.SuccessMessage = $"Tog bort inlägg med id {removeBlogPost.Id} och titel {removeBlogPost.Title}";
            }
            else
            {
                response.FailureMessage = $"Inlägget fanns inte";
                response.Success = false;
            }
            return response;
        }
    }
}
