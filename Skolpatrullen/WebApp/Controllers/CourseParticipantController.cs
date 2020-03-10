using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
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
            model.UserId = User.Id;

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
            courseParticipantVM.User = User;
            var response = await APIAddOrUpdateCourseParticipant(courseParticipantVM.ToCourseParticipant());
            SetResponseMessage(response);
            return RedirectToAction("CourseList", "Course");
        }
        [HttpGet]
        [Route("[controller]/AdminCourseParticipant")]
        public async Task<IActionResult> AdminCourseParticipant()
        {
            string message = await GetUser();
            if (User.IsSuperUser != true)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new AdminCourseParticipantViewModel();
            var userSchoolResponse = await APIGetAllUserSchools();
            if (userSchoolResponse != null)
            {
                //if (User.IsSuperUser == true)
                //{
                //    model.UserSchoolList = userSchoolResponse.Data;
                //}
                //else
                //{
                    model.UserSchoolList = userSchoolResponse.Data.Where(c => c.UserId == User.Id);
                //}
            }

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
            var userResponse = await APIGetAllUsers();
            if (userResponse != null)
            {
                model.UserList = userResponse.Data;
            }
            var schoolResponse = await APIGetAllSchools();
            if (schoolResponse != null)
            {
                model.SchoolList = schoolResponse.Data;
            }

            return View(model);
        }
        [HttpPost]
        [Route("[controller]/AdminCourseParticipant")]
        public async Task<IActionResult> Update(AdminCourseParticipantViewModel adminCourseParticipantVM, int Id, string answer)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(answer))
            {
                var cpResponse = await APIGetCourseParticipantById(Id);
                adminCourseParticipantVM.Id = Id;
                adminCourseParticipantVM.CourseId = cpResponse.Data.CourseId;
                adminCourseParticipantVM.Grade = cpResponse.Data.Grade;
                adminCourseParticipantVM.UserId = cpResponse.Data.UserId;

                switch (answer)
                {
                    case "Godkänn ansökan":
                        adminCourseParticipantVM.Status = Status.Accepted;
                        break;
                    case "Avslå ansökan":
                        adminCourseParticipantVM.Status = Status.NotApplied;
                        break;
                    case "Registrera som lärare":
                        adminCourseParticipantVM.Role = Roles.Teacher;
                        adminCourseParticipantVM.Status = Status.Accepted;
                        break;
                    default:
                        adminCourseParticipantVM.Status = Status.Applied;
                        break;
                }
            }
            var response = await APIAddOrUpdateCourseParticipant(adminCourseParticipantVM.ToCourseParticipant());
            SetResponseMessage(response);
            return RedirectToAction("AdminCourseParticipant", "CourseParticipant");
        }
    }
}