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
            var courseParticipantsResponse = APIGetAllCourseParticipants();
            if(courseParticipantsResponse != null)
            {
                model.PVM.CourseParticipantList = courseParticipantsResponse.Result.Data.Where(c=>c.UserId==User.Id).ToList();
            }
            var courseResponse = APIGetAllCourses();
            if (courseResponse != null)
            {
                model.PVM.CourseList = courseResponse.Result.Data.ToList();
            }

                return View(model);
        }

        [HttpPost]
        [Route("[controller]/Profile")]
        public async Task<IActionResult> ProfilePage(ProfileViewModel ProfileVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                User.Phone = ProfileVM.Phone;
                User.Email = ProfileVM.Email;
                User.Address = ProfileVM.Address;
                User.City = ProfileVM.City;
                User.PostalCode = ProfileVM.PostalCode;
                var response = await APIUpdateUser(User);
                if (response.Success)
                {

                    TempData["SuccessMessage"] = $"Ändringar sparade.";
                    return RedirectToAction("ProfilePage", "Profile");
                }
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("ProfilePage", "User");
        }
        [HttpGet]
        [Route("[controller]/AdminChangeProfile")]
        public async Task<IActionResult> AdminChangeProfile(int Id)
        {
            string message = await GetUser();
            var model = new AdminChangeProfileViewModel();
            var userResponse = await APIGetUserById(Id);
            if (userResponse.Data != null)
            {
                model.User = userResponse.Data;
                model.IsSuperUser = userResponse.Data.IsSuperUser;
            }
            model.UserId = Id;
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
            try
            {
                var userResponse = await APIGetUserById(changeProfile.UserId);
                if (userResponse.Success)
                {
                    var user = userResponse.Data;
                    user.Id = changeProfile.UserId;
                    user.FirstName = changeProfile.FirstName;
                    user.LastNames = changeProfile.LastNames;
                    user.SocialSecurityNr = changeProfile.SocialSecurityNr;
                    user.IsSuperUser = changeProfile.IsSuperUser;
                    var response = await APIUpdateUser(user);
                    if (response.Success)
                    {

                        TempData["SuccessMessage"] = $"Ändringar sparade för {user.FirstName} {user.LastNames}, {user.SocialSecurityNr}.";
                        return RedirectToAction("UserListPage", "Profile");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = response.ErrorMessages[0];
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = userResponse.ErrorMessages[0];
                }
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("UserListPage", "Profile");
        }
        [HttpGet]
        [Route("[controller]/UserList")]
        public async Task<IActionResult> UserListPage()
        {

            string message = await GetUser();
            var model = new List<User>();
            var userResponse = await APIGetAllUsers();
            if (userResponse.Data != null)
            {
                model = userResponse.Data.ToList();
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
            try
            {
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

                    if (response.Success)
                    {
                        TempData["SuccessMessage"] = "Ditt lösenord är nu ändrat!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = response.ErrorMessages[0];
                    }
                }
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("ProfilePage", "Profile");
        }
    }
}