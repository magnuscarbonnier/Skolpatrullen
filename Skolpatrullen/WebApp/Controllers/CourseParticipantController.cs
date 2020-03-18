using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
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

            var model = new AdminCourseParticipantViewModel();

            var userSchoolResponse = await APIGetAllUserSchools();
            var courseResponse = await APIGetAllCourses();
            var cpResponse = await APIGetAllCourseParticipants();
            var userResponse = await APIGetAllUsers();
            var schoolResponse = await APIGetAllSchools();

            if (userSchoolResponse != null && courseResponse != null && cpResponse != null && userResponse != null && schoolResponse != null)
            {
                if (User.IsSuperUser)
                {
                    //Method syntax
                    var response = cpResponse.Data
                        .Join(courseResponse.Data, cp => cp.CourseId, co => co.Id, (cp, co) => new { cp, co })
                        .Join(userResponse.Data, comb => comb.cp.UserId, us => us.Id, (comb, us) => new { comb.cp, comb.co, us })
                        .Join(schoolResponse.Data, comb => comb.co.SchoolId, sc => sc.Id, (comb, sc) => new { comb.cp, comb.co, comb.us, sc })
                        .Where(comb => comb.cp.Status == Status.Applied)
                        .OrderBy(comb => comb.cp.ApplicationDate)
                        .Select(comb => new CourseParticipant
                        {
                            ApplicationDate = comb.cp.ApplicationDate,
                            Course = comb.co,
                            CourseId = comb.cp.CourseId,
                            Grade = comb.cp.Grade,
                            Role = comb.cp.Role,
                            Status = comb.cp.Status,
                            Id = comb.cp.Id,
                            UserId = comb.cp.UserId,
                            User = comb.us
                        });
                    model.CourseParticipantList = response.ToList();
                    model.SchoolList = schoolResponse.Data;
                }
                else
                {
                    //Query syntax
                    var response = from cp in cpResponse.Data
                                   join co in courseResponse.Data on cp.CourseId equals co.Id
                                   join us in userResponse.Data on cp.UserId equals us.Id
                                   join sc in schoolResponse.Data on co.SchoolId equals sc.Id
                                   join usS in userSchoolResponse.Data on sc.Id equals usS.SchoolId
                                   where cp.Status == Status.Applied && usS.UserId == User.Id && usS.IsAdmin == true
                                   orderby cp.ApplicationDate ascending
                                   select new CourseParticipant
                                   {
                                       ApplicationDate = cp.ApplicationDate,
                                       Course = co,
                                       CourseId = cp.CourseId,
                                       Grade = cp.Grade,
                                       Role = cp.Role,
                                       Status = cp.Status,
                                       Id = cp.Id,
                                       UserId = cp.UserId,
                                       User = us
                                   };
                    model.CourseParticipantList = response.ToList();
                    model.SchoolList = schoolResponse.Data;
                }
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
                    case Constants.AcceptCP:
                        adminCourseParticipantVM.Status = Status.Accepted;
                        break;
                    case Constants.DenyCP:
                        adminCourseParticipantVM.Status = Status.NotApplied;
                        break;
                    case Constants.AcceptAsTeacherCP:
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
        [HttpGet]
        [Route("[controller]/CourseParticipantList")]
        public async Task<IActionResult> CourseParticipantList(int courseId)
        {
            string message = await GetUser();
            if (User != null)
            {
                var cpresponse = await APIGetAllCourseParticipants();
                var userresponse = await APIGetAllUsers();
                if (cpresponse == null || userresponse == null)
                {
                    return RedirectToAction("CourseList", "Course");
                }
                var response = cpresponse.Data
                            .Join(userresponse.Data, co => co.UserId, u => u.Id, (co, u) => new { co, u })
                            .Where(comb => comb.co.CourseId == courseId && comb.co.Status == Status.Accepted)
                            .OrderByDescending(comb => comb.co.Role)
                            .Select(comb => new CourseParticipantViewModel
                            {
                                Id = comb.co.Id,
                                CourseId=comb.co.CourseId,
                                Name = comb.u.FirstName + " " + comb.u.LastNames,
                                Role = comb.co.Role,
                                Status = comb.co.Status,
                                Grade = comb.co.Grade,
                                UserId=comb.co.UserId
                            });
                var course = await APIGetCourseById(courseId);
                var courseRole = await APIGetCourseRole(User.Id, courseId);
                var isSchoolAdmin = false;
                if (course.Data != null)
                {
                    var isSchoolAdminResponse = await APIIsSchoolAdmin(User.Id, course.Data.SchoolId);
                    isSchoolAdmin = isSchoolAdminResponse.Data;
                }
                if (User.IsSuperUser || isSchoolAdmin || courseRole.Data == Roles.Teacher)
                {
                    //returnera admin/lärarview
                    return View("EditCourseParticipantList", response);
                }
                else
                {
                    return View("CourseParticipantList", response);
                }
            }
            return RedirectToAction("CourseList", "Course");
        }
        [HttpGet]
        [Route("[controller]/EditCourseParticipant")]
        public async Task<IActionResult> EditCourseParticipant(int Id)
        {
            string message = await GetUser();
            if (User != null)
            {
                var cp = await APIGetCourseParticipantById(Id);
                var course = await APIGetCourseById(cp.Data.CourseId);
                var courseRole = await APIGetCourseRole(User.Id, cp.Data.CourseId);
                var isSchoolAdmin = false;
                if (course.Data != null)
                {
                    var isSchoolAdminResponse = await APIIsSchoolAdmin(User.Id, course.Data.SchoolId);
                    isSchoolAdmin = isSchoolAdminResponse.Data;
                }
                if (cp == null)
                {
                    return RedirectToAction("CourseList", "Course");
                }
                var model = new CourseParticipantViewModel();
                model.isSchoolAdmin = isSchoolAdmin;
                model.isSuperUser = User.IsSuperUser;
                model.isTeacher = courseRole.Data == Roles.Teacher;
                model.CourseId = cp.Data.CourseId;
                model.Grade = cp.Data.Grade;
                model.Id = cp.Data.Id;
                model.Role = cp.Data.Role;
                model.Status = cp.Data.Status;
                model.UserId = cp.Data.UserId;
                var userResponse = APIGetUserById(cp.Data.UserId);
                if(userResponse != null)
                {
                    model.User = userResponse.Result.Data;
                }
                return View(model);
               
            }
            return RedirectToAction("CourseParticipantList", "CourseParticipant");
        }
        [HttpPost]
        [Route("[controller]/EditCourseParticipant")]
        public async Task<IActionResult> EditCourseParticipant(CourseParticipantViewModel cpVM, int Id)
        {
            string message = await GetUser();

            if (!ModelState.IsValid)
            {
                return View();
            }
            CourseParticipant courseParticipant = (await APIGetCourseParticipantById(Id)).Data;
            if (courseParticipant == null)
            {
                return RedirectToAction("CourseParticipantList", "CourseParticipant", new { courseid = cpVM.CourseId });
            }
            var course = await APIGetCourseById(cpVM.CourseId);
            var courseRole = await APIGetCourseRole(User.Id, cpVM.CourseId);
            var isSchoolAdmin = false;
            if (course.Data != null)
            {
                var isSchoolAdminResponse = await APIIsSchoolAdmin(User.Id, course.Data.SchoolId);
                isSchoolAdmin = isSchoolAdminResponse.Data;
            }
            if (isSchoolAdmin||User.IsSuperUser)
            {
                courseParticipant.Status = cpVM.Status;
                courseParticipant.Role = cpVM.Role;
                if (courseParticipant.Role != Roles.Teacher)
                {
                    courseParticipant.Grade = cpVM.Grade;
                }
                else
                {
                    courseParticipant.Grade = "";
                }
            }
            else if (courseRole.Data == Roles.Teacher)
            {
                if (courseParticipant.UserId != User.Id && courseParticipant.Role != Roles.Teacher)
                {
                    courseParticipant.Grade = cpVM.Grade;
                }
            }
            else
            {
                return RedirectToAction("CourseParticipantList", "CourseParticipant", new { courseid = cpVM.CourseId });
            }

            var response = await APIAddOrUpdateCourseParticipant(courseParticipant);

            SetResponseMessage(response);
            return RedirectToAction("CourseParticipantList", "CourseParticipant", new { courseid = cpVM.CourseId } );
        }
    }
}