using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CourseController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> AddCoursePage()
        {
            string message = await GetUser();
            if (User != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> AddCoursePage(CourseViewModel CourseVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                var response = await APIAddCourse(CourseVM.ToCourse());
                return RedirectToAction("AddCoursePage", "Course");
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("AddCoursePage", "Course");

        }
    }
}