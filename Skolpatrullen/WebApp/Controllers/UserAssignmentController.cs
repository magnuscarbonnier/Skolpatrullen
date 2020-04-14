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
    }
}