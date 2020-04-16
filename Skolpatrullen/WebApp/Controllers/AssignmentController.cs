using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AssignmentController : AppController
    {
        [HttpPost]
        [Route("[controller]/AddCourseAssignment")]
        public async Task<IActionResult> AddCourseAssignment(AssignmentViewModel assignment, int courseId)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            Assignment newAssignmnet = new Assignment();
            newAssignmnet.CourseId = courseId;
            newAssignmnet.Deadline = assignment.Deadline;
            newAssignmnet.Description = assignment.Description;
            newAssignmnet.Name = assignment.Name;

            var response = await APIAddAssignment(newAssignmnet);

            if (assignment.File != null && assignment.File.Any())
            {
                foreach (var file in assignment.File)
                {
                    AssignmentFileBody body = new AssignmentFileBody();
                    byte[] bytefile = null;
                    using (var filestream = file.OpenReadStream())
                    using (var memstream = new MemoryStream())
                    {
                        filestream.CopyTo(memstream);
                        bytefile = memstream.ToArray();
                    }

                    body.File = bytefile;
                    body.UploadDate = DateTime.Now;
                    body.UserId = User.Id;
                    body.AssignmentId = response.Data.Id;
                    body.ContentType = file.ContentType;
                    body.Name = file.FileName;
                    body.Type = FileTypes.Assignment;

                    await APIUploadAssignmentFile(body);
                }
            }
            return RedirectToAction("GetCourseById", "course", new { Id = assignment.CourseId });
        }
        [HttpGet]
        [Route("[controller]/CourseAssignments/{courseid}")]
        public async Task<IActionResult> CourseAssignments(int courseId)
        {
            string message = await GetUser();
            if (User != null)
            {
                IEnumerable<Assignment> assignments = new List<Assignment>();
                var response = await APIGetAssignmentByCourseId(courseId);
                if (response.Data != null)
                {
                    assignments = response.Data.Select(a => new Assignment()
                    {
                        Id = a.Id,
                        CourseId = courseId,
                        Name = a.Name,
                        Deadline = a.Deadline,
                        Description = a.Description
                    });
                    return View(assignments);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("[controller]/{id}")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            string message = await GetUser();
            var model = new AssignmentViewModel();
            var assignment = await APIGetAssignmentById(id);

            if (assignment.Data != null)
            {
                var courseparticipantresponse = APIGetCourseParticipantsByUserId(User.Id).Result.Data.SingleOrDefault(us=>us.CourseId==assignment.Data.CourseId);
                if(courseparticipantresponse != null && courseparticipantresponse.Role==Roles.Lärare)
                {
                    model.IsTeacher = true;
                }
                else
                {
                    model.IsTeacher = false;
                }
                var assignmentfiles = await APIGetFilesByAssignment(id);
                var turnedin = await APIUserAssignmentReturnedStatus(id, User.Id);
                model.AssignmentFiles = assignmentfiles.Data.Where(af => af.Type == FileTypes.Assignment);
                model.CourseId = assignment.Data.CourseId;
                model.Deadline = assignment.Data.Deadline;
                model.Description = assignment.Data.Description;
                model.Name = assignment.Data.Name;
                model.Id = assignment.Data.Id;
                model.TurnedIn = turnedin.Data;

                return View("AssignmentDetails", model);
            }
            else
            {
                return NotFound();
            }
        }
    }
}