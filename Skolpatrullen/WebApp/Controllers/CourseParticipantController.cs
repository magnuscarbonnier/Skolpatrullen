using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CourseParticipantController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> AddCourseParticipant(int Id)
        {
            string message = await GetUser();
            var model = new CourseParticipantViewModel();
            model.UserId= User.Id;

            var courseResponse = await APIGetCourseById(Id);
            if (courseResponse.Data != null)
            {
                model.Course = courseResponse.Data;
                model.CourseId = courseResponse.Data.Id;
            }
            return View(model);
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> AddCourseParticipant(CourseParticipantViewModel courseParticipantVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                courseParticipantVM.User = User;
                var response = await APIAddOrUpdateCourseParticipant(courseParticipantVM.ToCourseParticipant());
                TempData["SuccessMessage"] = $"Du har ansökt till kursen.";
                return RedirectToAction("CourseList", "Course");
            }
            catch
            {
                //send to error?
                TempData["ErrorMessage"] = $"Något gick fel..";
            }
            return RedirectToAction("CourseList", "Course");
        }
    }
}