using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class HomeController : AppController
    {
        //public async Task<IActionResult> Index()
        //{
        //    string message = await GetUser();
        //    return View();
        //}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string message = await GetUser();
            var model = new StartBlogViewModel();

            var startBlog = await APIGetAllStartBlogPosts();
            
            if (startBlog.Data != null)
            {
                model.BlogPosts = startBlog.Data.OrderByDescending(bp=>bp.PublishDate).Take(5);
            }
            return View(model);
        }

        public async Task<IActionResult> Privacy()
        {
            string message = await GetUser();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            string message = await GetUser();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("[controller]/RemoveStartBlogPost/{id}")]
        public async Task<IActionResult> RemoveStartBlogPost(int Id)
        {
            string message = await GetUser();
            
            var response = await APIRemoveStartBlogPost(Id);
            SetResponseMessage(response);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("[controller]/AddStartBlogPost")]
        public async Task<IActionResult> AddStartBlogPost(StartBlogPost blogPost)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            blogPost.UserId = User.Id;
            blogPost.PublishDate = DateTime.Now;
            var response = await APIAddStartBlogPost(blogPost);

            return RedirectToAction("Index");
        }

    }
}
