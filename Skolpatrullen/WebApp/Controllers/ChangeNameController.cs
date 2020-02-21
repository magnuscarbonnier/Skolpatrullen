using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ChangeNameController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> ChangeNamePage()
        {
            string message = await GetUser();
            var model = new ChangeNameViewModel();
            model.User = User;

            return View(model);
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> ChangeNamePage(ChangeNameViewModel changeNameVM)
        {

            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                User.FirstName = changeNameVM.FirstName;
                User.LastNames = changeNameVM.LastNames;

                var response = await APIUpdateUser(User);
                if (response.Success)
                {
                    return RedirectToAction("ChangeNamePage", "ChangeName");
                }
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("ChangeNamePage", "ChangeName");
        }
    }
}