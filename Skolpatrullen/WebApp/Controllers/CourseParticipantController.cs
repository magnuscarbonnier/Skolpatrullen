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
            var cpResponse = await APIGetCourseParticipantsByUserId(User.Id);
            var courseParticipant = cpResponse.Data.SingleOrDefault(cp => cp.CourseId == Id);
            if (courseParticipant != null)
            {
                model.Status = courseParticipant.Status;
            }
            else
            {
                model.Status = Database.Models.Status.NotApplied;
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
        [HttpGet]
        [Route("Admin/CourseParticipants")]
        public async Task<IActionResult> AdminCourseParticipant()
        {
            string message = await GetUser();
            var model = new AdminCourseParticipantViewModel();
            
            var courseResponse = await APIGetAllCourses();
            if (courseResponse.Data != null)
            {
                model.CourseList = courseResponse.Data;
            }
            var cpResponse = await APIGetAllCourseParticipants();
            if (cpResponse != null)
            {
                model.CourseParticipantList = cpResponse.Data;
            }
            
            return View(model);
        }
    }
}