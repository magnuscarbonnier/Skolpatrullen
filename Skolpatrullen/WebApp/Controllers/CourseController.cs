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
        [Route("[controller]/SuperCourseList")]
        public async Task<IActionResult> SuperCourseList()
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
            if(User != null)
            {
                var model = new CourseViewModel();
                var response = await APIGetAllSchools();
                if(response.Data != null)
                {
                    model.SchoolList = response.Data.ToList();
                    model.StartDate = DateTime.Now.ToLocalTime();
                    model.EndDate = DateTime.Now.AddMonths(1).ToLocalTime();
                    return View(model);
                }
            }

            return RedirectToAction("CourseList", "Course");
        }
        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> AddCourse(CourseViewModel course)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (course.StartDate > course.EndDate)
            {
                TempData["ErrorMessage"] = "Startdatum kan inte vara senare än slutdatum";
            }
            try
            {
                if(course.EndDate > course.StartDate)
                {
                    var response = await APIAddCourse(course.ToCourse());
                    if (response.Success)
                    {
                        TempData["SuccessMessage"] = $"Kurs tillagd.";
                        return RedirectToAction("CourseList", "Course");
                    }
                } else
                {
                    TempData["ErrorMessage"] = "Startdatum måste vara senare än slutdatum.";
                    return RedirectToAction("AddCoursePage", "Course");
                }
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("AddCoursePage", "Course");
        }

        [HttpGet]
        [Route("[controller]/Remove/{id}")]
        public async Task<IActionResult> RemoveCourse(int id)
        {
            try
            {
                var response = await APIRemoveCourse(id);
                if (response.Success)
                {
                    TempData["SuccessMessage"] = $"Kurs borttagen";
                    return RedirectToAction("CourseList", "Course");
                }
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("CourseList", "Course");
        }
        [HttpPost]
        [Route("[controller]/CourseList/{id}")]
        public async Task<IActionResult> CourseList(string id)
        {
            string message = await GetUser();
            var model = new CourseListViewModel();
            var courseResponse = await APIGetAllCourses();
            if (courseResponse.Data != null)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    model.CourseList = courseResponse.Data.Where(s => s.Name.Contains(id));
                }
                else
                    model.CourseList = courseResponse.Data;
            }
            var schoolResponse = await APIGetAllSchools();
            if (schoolResponse.Data != null)
            {
                model.SchoolList = schoolResponse.Data;
            }
            return View(model);
        }
    }
}