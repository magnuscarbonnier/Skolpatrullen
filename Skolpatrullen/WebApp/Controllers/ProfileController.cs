using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProfileController : AppController
    {

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> ProfilePage()
        {
            string message = await GetUser();
            var model = new ProfileViewModel();
            model.User = User;

            return View(model);
        }

        [HttpPost]
        [Route("[controller]")]
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
                    return RedirectToAction("ProfilePage", "Profile");
                }
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("ProfilePage", "User");
        }

        [HttpPost]
        [Route("[controller]/ChangePassword")]
        public async Task<IActionResult> ChangePassword(ProfileViewModel ProfileVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                if (!(ProfileVM.NewPassword == ProfileVM.ReNewPassword))
                {
                    ProfileVM.NewPassword = "";
                    ProfileVM.ReNewPassword = "";
                    TempData["ErrorMessage"] = $"Lösenordet matchade inte, försök igen.";
                }
                else
                {
                    ChangePasswordBody body = new ChangePasswordBody();
                    body.UserId = User.Id;
                    body.CurrentPassword = ProfileVM.Password;
                    body.NewPassword = ProfileVM.NewPassword;

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