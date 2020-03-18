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
            if (User != null)
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
            if (User != null)
            {
                var model = new CourseViewModel();
                var response = await APIGetAllSchools();
                if (response.Data != null)
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
                SetFailureMessage("Startdatum kan inte vara senare än slutdatum");
                return RedirectToAction("AddCoursePage", "Course");
            }
            var response = await APIAddCourse(course.ToCourse());
            return SetResponseMessage(response, RedirectToAction("CourseList", "Course"), RedirectToAction("AddCoursePage", "Course"));
        }

        [HttpGet]
        [Route("[controller]/Remove/{id}")]
        public async Task<IActionResult> RemoveCourse(int id)
        {
            var response = await APIRemoveCourse(id);
            SetResponseMessage(response);
            return RedirectToAction("CourseList", "Course");
        }
        [HttpPost]
        [Route("[controller]/SearchCourses")]
        public async Task<IActionResult> SearchCourses(CourseListViewModel courseVM)
        {
            string message = await GetUser();
            var model = new CourseListViewModel();
            var courseResponse = await APIGetAllCourses();
            if (courseResponse.Data != null)
            {
                if (!string.IsNullOrEmpty(courseVM.Search))
                {
                    model.CourseList = courseResponse.Data.Where(s => s.Name.ToLower().Contains(courseVM.Search.ToLower()));
                }
                else
                {
                    model.CourseList = courseResponse.Data;
                }
            }
            var schoolResponse = await APIGetAllSchools();
            if (schoolResponse.Data != null)
            {
                model.SchoolList = schoolResponse.Data;
            }
            return View("CourseList", model);
        }
        [HttpGet]
        [Route("[controller]/CourseList")]
        public async Task<IActionResult> CourseList()
        {
            string message = await GetUser();
            var model = new CourseListViewModel();
            var courseResponse = await APIGetAllCourses();
            if (courseResponse.Data != null)
            {
                model.CourseList = courseResponse.Data;
            }
            var schoolResponse = await APIGetAllSchools();
            if (schoolResponse.Data != null)
            {
                model.SchoolList = schoolResponse.Data;
            }
            return View(model);
        }
        [HttpGet]
        [Route("[controller]/{id}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            string message = await GetUser();
            var model = new Course();

            var course = await APIGetCourseById(courseId);
            var courseRole = await APIGetCourseRole(User.Id, courseId);
            var isSchoolAdmin = false;
            if (course.Data != null)
            {
                var isSchoolAdminResponse = await APIIsSchoolAdmin(User.Id, course.Data.SchoolId);
                isSchoolAdmin = isSchoolAdminResponse.Data;
                model = course.Data;
            }
            if (User.IsSuperUser || isSchoolAdmin || courseRole.Data == Roles.Teacher)
            {
                return View("AdminCourseDetails", model);
            }
            else
            {
                return View("CourseDetails", model);
            }
        }
    }
}