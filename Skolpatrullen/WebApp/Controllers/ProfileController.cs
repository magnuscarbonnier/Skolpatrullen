using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var model = new ProfileViewModel();
            model.User = User;

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
        [Route("[controller]/ChangeName")]
        public async Task<IActionResult> ChangeNamePage(int Id)
        {
            string message = await GetUser();
            var model = new ChangeNameViewModel();
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
        [Route("[controller]/ChangeName")]
        public async Task<IActionResult> ChangeNamePage(ChangeNameViewModel changeNameVM)
        {

            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                var userResponse = await APIGetUserById(changeNameVM.UserId);
                if (userResponse.Success)
                {
                    var user = userResponse.Data;
                    user.Id = changeNameVM.UserId;
                    user.FirstName = changeNameVM.FirstName;
                    user.LastNames = changeNameVM.LastNames;
                    user.SocialSecurityNr = changeNameVM.SocialSecurityNr;
                    user.IsSuperUser = changeNameVM.IsSuperUser;
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
            var model = new UserListViewModel();
            var userResponse = await APIGetAllUsers();
            if (userResponse.Data != null)
            {
                model.UserList = userResponse.Data;
            }
            return View(model);

        }
    }
}