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
                var response = await APIUpdateUser(ProfileVM.ToUser());
                return RedirectToAction("ProfilePage", "Profile");
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("ProfilePage", "User");
        }
    }
}