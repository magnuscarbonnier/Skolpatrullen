using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CourseController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> CourseList()
        {
            string message = await GetUser();
            if(User != null)
            {
                IEnumerable<Course> courses = new List<Course>();
                var response = await APIGetAllCourses();
                if (response.Data != null)
                {
                    courses = response.Data;
                }
                return View(courses);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddCoursePage()
        {
            string message = await GetUser();
            var model = new CourseViewModel();
            var response = await APIGetAllSchools();
            if (response.Data != null)
            {
                model.SchoolList = response.Data;
                model.StartDate = DateTime.Now.ToLocalTime();
                model.EndDate = DateTime.Now.AddMonths(1).ToLocalTime();
            }
            return View(model);

        }
        [HttpPost]
        [Route("[controller]/Add")]
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
                if (response.Success)
                {
                    TempData["SuccessMessage"] = $"Kurs tillagd.";
                    return RedirectToAction("AddCoursePage", "Course");
                }
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("AddCoursePage", "Course");

        }
    }
}