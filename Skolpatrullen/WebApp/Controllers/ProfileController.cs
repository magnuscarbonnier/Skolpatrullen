using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

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
            return RedirectToAction("ProfilePage", "User");
        }
        [HttpGet]
        [Route("[controller]/AdminChangeProfile")]
        public async Task<IActionResult> AdminChangeProfile(int Id)
        {
            string message = await GetUser();
            var model = new AdminChangeProfileViewModel();
            var response = await APIGetUserById(Id);
            if (response.Data != null)
            {
                model.User = response.Data;
                model.IsSuperUser = response.Data.IsSuperUser;
            }
            model.UserId = Id;
            SetResponseMessage(response);
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
            SetResponseMessage(response);
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
    }
}