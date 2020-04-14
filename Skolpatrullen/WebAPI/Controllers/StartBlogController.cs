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
    public class StartBlogController : APIController
    {
        public StartBlogController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }
        [HttpGet]
        [Route("[controller]/GetAll")]
        public APIResponse<IEnumerable<StartBlogPost>> GetAll()
        {
            APIResponse<IEnumerable<StartBlogPost>> response = new APIResponse<IEnumerable<StartBlogPost>>();

            var startBlogList = _context.StartBlogPosts;
            if (startBlogList != null)
            {
                response.Data = startBlogList;
                response.Success = true;
                response.SuccessMessage = $"Hämtade alla inlägg";
            }
            else
            {
                response.Success = false;
                response.FailureMessage = "Kunde inte hämta inlägg";
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public APIResponse<StartBlogPost> Add(StartBlogPost blogPost)
        {
            APIResponse<StartBlogPost> response = new APIResponse<StartBlogPost>();
            if (blogPost != null)
            {
                _context.StartBlogPosts.Add(blogPost);
                _context.SaveChanges();
                response.Data = blogPost;
                response.Success = true;
                response.SuccessMessage = $"La till inlägg med titel: {blogPost.Title}";
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
            var removeBlogPost = _context.StartBlogPosts.SingleOrDefault(s => s.Id == id);
            if (removeBlogPost != null)
            {
                _context.Remove(removeBlogPost);
                _context.SaveChanges();
                response.Success = true;
                response.SuccessMessage = $"Tog bort inlägg med titel: {removeBlogPost.Title}";
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
