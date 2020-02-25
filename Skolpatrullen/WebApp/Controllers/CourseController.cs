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
        [Route("[controller]")]
        public async Task<IActionResult> AddCoursePage(CourseViewModel CourseVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (CourseVM.StartDate > CourseVM.EndDate)
            {
                TempData["ErrorMessage"] = "Startdatum kan inte vara senare än slutdatum";
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