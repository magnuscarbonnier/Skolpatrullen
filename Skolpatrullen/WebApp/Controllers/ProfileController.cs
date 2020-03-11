using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;

namespace WebApp.Controllers
{
    public class ProfileController : AppController
    {

        [HttpGet]
        [Route("[controller]/Profile")]
        public async Task<IActionResult> ProfilePage()
        {
            string message = await GetUser();
            var model = new ProfileCombinedViewModel();
            model.PVM.User = User;
            if (User.ProfilePictureId != null)
            {
                model.PVM.User.ProfilePicture = (await APIGetFileById((int)User.ProfilePictureId)).Data;
            }
            var courseParticipantsResponse = APIGetAllCourseParticipants();
            if (courseParticipantsResponse != null)
            {
                model.PVM.CourseParticipantList = courseParticipantsResponse.Result.Data.Where(c => c.UserId == User.Id).ToList();
            }
            var courseResponse = APIGetAllCourses();
            if (courseResponse != null)
            {
                model.PVM.CourseList = courseResponse.Result.Data.ToList();
            }

            return View(model);
        }

        [HttpPost]
        [Route("[controller]/ChangeProfile")]
        public async Task<IActionResult> ChangeProfile(ProfileViewModel ProfileVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            User = ProfileVM.UpdateUser(User);
            var response = await APIUpdateUser(User);
            SetResponseMessage(response);
            return RedirectToAction("ProfilePage", "Profile");
        }
        [HttpGet]
        [Route("[controller]/AdminChangeProfile")]
        public async Task<IActionResult> AdminChangeProfile(int Id)
        {
            string message = await GetUser();
            var model = new AdminChangeProfileViewModel();
            var response = await APIGetUserById(Id);

            if (response.Data.ProfilePictureId != null)
            {
                response.Data.ProfilePicture = (await APIGetFileById((int)response.Data.ProfilePictureId)).Data;
            }

            if (response.Data != null)
            {
                model.User = response.Data;
                model.IsSuperUser = response.Data.IsSuperUser;
            }
            model.UserId = Id;
            if (!string.IsNullOrEmpty(response.FailureMessage))
            {
                SetFailureMessage(response.FailureMessage);
            }
            return View(model);
        }
        [HttpPost]
        [Route("[controller]/AdminChangeProfile")]
        public async Task<IActionResult> AdminChangeProfile(AdminChangeProfileViewModel changeProfile)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userResponse = await APIGetUserById(changeProfile.UserId);
            if (userResponse.Success)
            {
                var user = changeProfile.UpdateUser(userResponse.Data);
                var response = await APIUpdateUser(user);
                SetResponseMessage(response);
            }
            else
            {
                SetFailureMessage(userResponse.FailureMessage);
            }
            return RedirectToAction("UserListPage", "Profile");
        }
        [HttpGet]
        [Route("[controller]/UserList")]
        public async Task<IActionResult> UserListPage()
        {

            string message = await GetUser();
            var model = new List<User>();
            var response = await APIGetAllUsers();
            if (response.Data != null)
            {
                model = response.Data.ToList();
            }
            if (!string.IsNullOrEmpty(response.FailureMessage))
            {
                SetFailureMessage(response.FailureMessage);
            }
            return View(model);

        }
        [HttpPost]
        [Route("[controller]/ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel ChangePasswordVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!(ChangePasswordVM.NewPassword == ChangePasswordVM.ReNewPassword))
            {
                ChangePasswordVM.NewPassword = "";
                ChangePasswordVM.ReNewPassword = "";
                TempData["ErrorMessage"] = $"Lösenorden matchade inte, försök igen.";
            }
            else
            {
                ChangePasswordBody body = new ChangePasswordBody();
                body.UserId = User.Id;
                body.CurrentPassword = ChangePasswordVM.Password;
                body.NewPassword = ChangePasswordVM.NewPassword;

                var response = await APIChangePassword(body);
                SetResponseMessage(response);
            }

            return RedirectToAction("ProfilePage", "Profile");
        }


        [HttpPost]
        [Route("[controller]/ChangeProfilePicture")]
        public async Task<IActionResult> ChangeProfilePicture(ChangeProfilePictureViewModel vm)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (vm.file != null && vm.file.Length > 0)
            {
                ChangeProfilePictureBody body = new ChangeProfilePictureBody();

                byte[] p1 = null;
                using (var fs1 = vm.file.OpenReadStream())
                using (var ms1 = new MemoryStream())
                {
                    fs1.CopyTo(ms1);
                    p1 = ms1.ToArray();
                }

                body.UserId = User.Id;
                body.ProfilePicture = p1;
                body.UploadDate = DateTime.Now;
                body.FileExtension = "test";

                var response = await APIChangeProfilePicture(body);
            }
            else if(vm.file == null)
            {
                var fileResponse = await APIGetFileById(Convert.ToInt32(User.ProfilePictureId));
                var response = await APIDeleteFileById(fileResponse.Data.Id);
            }
            return RedirectToAction("ProfilePage", "Profile");


        }

        [HttpPost]
        [Route("[controller]/AdminRemoveProfilePicture")]
        public async Task<IActionResult> AdminRemoveProfilePicture(RemoveProfilePicture vm)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userResponse = await APIGetUserById(vm.UserId);
            var response = await APIDeleteFileById(Convert.ToInt32(userResponse.Data.ProfilePictureId));

            return RedirectToAction("UserListPage", "Profile");


        }
    }
}