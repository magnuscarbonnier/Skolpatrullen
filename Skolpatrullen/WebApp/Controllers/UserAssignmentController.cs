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
    public class UserAssignmentController : AppController
    {
        [HttpGet]
        [Route("[controller]/{assignmentId}")]
        public async Task<IActionResult> AddUserAssignment(int assignmentId)
        {
            string message = await GetUser();
            var model = new UserAssignmentViewModel();
            var response = await APIGetAssignmentById(assignmentId);

            //not being used atm, can be used to display that you already turned it in.
            var turnedin = await APIUserAssignmentReturnedStatus(assignmentId, User.Id);

            if (response.Data != null)
            {
                model.TurnedIn = turnedin.Data;
                model.Assignment = response.Data;
            }
            return View(model);
        }
        [HttpPost]
        [Route("[controller]/{assignmentId}")]
        public async Task<IActionResult> AddUserAssignment(UserAssignmentViewModel vm)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userassignment = new UserAssignment();
            userassignment.Description = vm.Description;
            userassignment.ReturnDate = DateTime.Now;
            userassignment.UserId = User.Id;
            userassignment.AssignmentId = vm.AssignmentId;

            var response = await APIAddOrUpdateUserAssignment(userassignment);
            if (response.Success)
                SetSuccessMessage(response.SuccessMessage);
            else
                SetFailureMessage(response.FailureMessage);

            if (vm.Files != null && vm.Files.Any())
            {
                foreach (var file in vm.Files)
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
                    body.AssignmentId = vm.AssignmentId;
                    body.ContentType = file.ContentType;
                    body.Name = file.FileName;
                    body.Type = FileTypes.UserAssignment;

                    await APIUploadAssignmentFile(body);
                }
            }
            return RedirectToAction("GetAssignmentById", "Assignment", new { id = vm.AssignmentId });
        }
        [HttpGet]
        [Route("[controller]/UserAssignmentList/{assignmentId}")]
        public async Task<IActionResult> UserAssignmentList(int assignmentId)
        {
            string message = await GetUser();
            var model = new UserAssignmentListViewModel();
            var UAresponse = await APIGetAllUserAssignmentsByAssignmentId(assignmentId);
            var assignmentresponse = await APIGetAssignmentById(assignmentId);
            if (assignmentresponse.Data != null && UAresponse.Data != null)
            {
                var userresponse = await APIGetStudentsByCourseId(assignmentresponse.Data.CourseId);
                if (userresponse.Data != null)
                {
                    var result = from ua in UAresponse.Data
                                 join us in userresponse.Data on ua.UserId equals us.Id
                                 orderby ua.ReturnDate ascending
                                 select new UserAssignment
                                 {
                                     AssignmentId = ua.AssignmentId,
                                     Description = ua.Description,
                                     Grade = ua.Grade,
                                     Id = ua.Id,
                                     ReturnDate = ua.ReturnDate,
                                     UserId = ua.UserId,
                                     User = us
                                 };

                    model.UserAssignments = result;
                    model.Users = userresponse.Data;
                    model.Assignment = assignmentresponse.Data;
                }
            }
            return View(model);
        }
        [HttpGet]
        [Route("[controller]/EditUserAssignment/{CourseId}/{AssignmentId}/{UserId}")]
        public async Task<IActionResult> EditUserAssignment(int CourseId, int AssignmentId, int UserId)
        {
            string message = await GetUser();
            var model = new UserAssignmentViewModel();
            var UAresponse = await APIGetUserAssignmentByCourseUserAndAssignment(CourseId, UserId, AssignmentId);
            var userresponse = await APIGetUserById(UserId);
            var assignmentresponse = await APIGetAssignmentById(AssignmentId);
            model.TurnedIn = (await APIUserAssignmentReturnedStatus(AssignmentId, UserId)).Data;
            var userassignmentfiles = await APIGetUserAssignmentFilesByUserId(UserId);

            if (UAresponse.Data != null && userresponse.Data != null)
            {
                model.AssignmentFiles = userassignmentfiles.Data.Where(ua => ua.Type == AssignmentFileType.StudentFile && ua.AssignmentId == AssignmentId).Select(ua => ua.File);
                model.AssignmentId = UAresponse.Data.AssignmentId;
                model.Assignment = UAresponse.Data.Assignment;
                model.Description = UAresponse.Data.Description;
                model.Grade = UAresponse.Data.Grade;
                model.ReturnDate = UAresponse.Data.ReturnDate;
                model.UserId = UAresponse.Data.UserId;
                model.User = userresponse.Data;
            }
            else
            {
                model.UserId = UserId;
                model.AssignmentId = AssignmentId;
                if (assignmentresponse.Data != null)
                    model.Assignment = assignmentresponse.Data;
                if (userresponse.Data != null)
                    model.User = userresponse.Data;
            }
            return View(model);
        }
        [HttpPost]
        [Route("[controller]/EditUserAssignment/{CourseId}/{AssignmentId}/{UserId}")]
        public async Task<IActionResult> EditUserAssignment(UserAssignment userAssignment)
        {
            string message = await GetUser();
            var response = await APIAddOrUpdateUserAssignment(userAssignment);

            if (response.Success)
            {
                SetSuccessMessage(response.SuccessMessage);
            }
            else
            {
                SetFailureMessage(response.FailureMessage);
            }

            return RedirectToAction("UserAssignmentList", "UserAssignment", new { assignmentId = userAssignment.AssignmentId });
        }
    }
}