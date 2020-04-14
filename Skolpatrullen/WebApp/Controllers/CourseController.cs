using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
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
                var courseresponse = await APIGetAllCourses();
                var schoolresponse = await APIGetAllSchools();
                if (courseresponse.Data != null && schoolresponse.Data != null)
                {
                    var response= from co in courseresponse.Data
                                  join sc in schoolresponse.Data on co.SchoolId equals sc.Id
                                  select new Course
                                  {
                                      Id=co.Id, Name=co.Name, SchoolId=co.SchoolId, StartDate=co.StartDate, EndDate=co.EndDate, School=sc
                                  };
                    return View(response);
                }
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
            return RedirectToAction("SuperCourseList", "Course");
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
        public async Task<IActionResult> GetCourseById(int id)
        {
            string message = await GetUser();
            var model = new Course();

            var course = await APIGetCourseById(id);
            var courseBlog = await APIGetBlogPostsByCourseId(id);
            var courseRole = await APIGetCourseRole(User.Id, id);
            var isSchoolAdmin = false;
            if (course.Data != null)
            {
                var isSchoolAdminResponse = await APIIsSchoolAdmin(User.Id, course.Data.SchoolId);
                isSchoolAdmin = isSchoolAdminResponse.Data;
                model = course.Data;
            }
            if (User.IsSuperUser || isSchoolAdmin || courseRole.Data == Roles.Lärare)
            {
                if (courseBlog.Data != null)
                    model.CourseBlogPosts = courseBlog.Data.OrderByDescending(cb => cb.PublishDate);
                return View("AdminCourseDetails", model);
            }
            else
            {
                if (courseBlog.Data != null)
                    model.CourseBlogPosts = courseBlog.Data.OrderByDescending(cb => cb.PublishDate);
                return View("CourseDetails", model);
            }
        }
        [HttpPost]
        [Route("[controller]/UploadCourseFile")]
        public async Task<IActionResult> UploadCourseFile(UploadCourseFileViewModel vm, int courseId)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (vm.File != null && vm.File.Length > 0)
            {
                CourseFileBody body = new CourseFileBody();
                byte[] bytefile = null;
                using (var filestream = vm.File.OpenReadStream())
                using (var memstream = new MemoryStream())
                {
                    filestream.CopyTo(memstream);
                    bytefile = memstream.ToArray();
                }

                body.File = bytefile;
                body.UploadDate = DateTime.Now;
                body.UserId = User.Id;
                body.CourseId = courseId;
                body.ContentType = vm.File.ContentType;
                body.Name = vm.File.FileName;

                var response = await APIUploadCourseFile(body);
            }
            return RedirectToAction("GetCourseById", new { Id = courseId });
        }
        [HttpGet]
        [Route("[controller]/CourseFiles/{courseid}")]
        public async Task<IActionResult> CourseFiles(int courseId)
        {
            string message = await GetUser();
            if (User != null)
            {
                IEnumerable<CourseFileBody> files = new List<CourseFileBody>();
                var response = await APIGetAllCourseFiles(courseId);
                if (response.Data != null)
                {
                    files = response.Data.Select(f => new CourseFileBody()
                    {
                        Id = f.Id,
                        CourseId = courseId,
                        File = f.Binary,
                        Name = f.Name,
                        ContentType = f.ContentType,
                        UploadDate = f.UploadDate
                    });
                    return View(files);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("[controller]/DownloadFile/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var file = await APIGetFileById(id);
            if (file != null)
            {
                return File(file.Data.Binary, file.Data.ContentType, file.Data.Name);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [Route("[controller]/AddCourseBlogPost")]
        public async Task<IActionResult> AddCourseBlogPost(CourseBlogPost blogPost, int courseId)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            blogPost.CourseId = courseId;
            blogPost.UserId = User.Id;
            blogPost.PublishDate = DateTime.Now;
            var response = await APIAddBlogPost(blogPost);

            return RedirectToAction("GetCourseById", new { Id = blogPost.CourseId });
        }
        [HttpGet]
        [Route("[controller]/RemoveCourseBlogPost/{id}")]
        public async Task<IActionResult> RemoveCourseBlogPost(int Id, int CourseId)
        {
            var response = await APIRemoveBlogPost(Id);
            SetResponseMessage(response);
            return RedirectToAction("GetCourseById", new { id = CourseId});
        }
        [HttpGet]
        [Route("[controller]/UserCourseList")]
        public async Task<IActionResult> UserCourseList()
        {
            string message = await GetUser();
            var model = new UserCourseListViewModel();
            var courseParticipantsResponse = await APIGetCourseParticipantsByUserId(User.Id);
            var courseResponse = await APIGetCoursesByUserId(User.Id);
            var schoolResponse = await APIGetSchoolsByUserId(User.Id);
            if (courseParticipantsResponse.Data != null && courseResponse.Data != null && schoolResponse.Data != null)
            {
                var courseParticipants = from cp in courseParticipantsResponse.Data
                                         join co in courseResponse.Data on cp.CourseId equals co.Id
                                         orderby cp.ApplicationDate ascending, cp.Status
                                         select new CourseParticipant
                                         {
                                             ApplicationDate = cp.ApplicationDate,
                                             Course = co,
                                             CourseId = cp.CourseId,
                                             Grade = cp.Grade,
                                             Role = cp.Role,
                                             Status = cp.Status,
                                             Id = cp.Id,
                                         };
                model.CourseParticipantList = courseParticipants.ToList();
                model.SchoolList = schoolResponse.Data.ToList();
            }
            return View(model);
        }
        [HttpGet]
        [Route("[controller]/SchoolCourseList/{SchoolId}")]
        public async Task<IActionResult> SchoolCourseList(int SchoolId)
        {
            string message = await GetUser();
            var model = new SchoolCourseListViewModel();
            var courseResponse = await APIGetCoursesBySchoolId(SchoolId);
            if (courseResponse.Data != null)
            {
                model.CourseList = courseResponse.Data;
            }
            var schoolResponse = await APIGetAllSchools();
            if (schoolResponse.Data != null)
            {
                model.School = schoolResponse.Data.SingleOrDefault(sc=>sc.Id==SchoolId);
            }
            return View(model);
        }
    }
}