using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class PasswordRecoveryController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> RecoverPassword()
        {
            var message = await GetUser();
            if (User != null)
            {
                SetFailureMessage("Du är redan inloggad");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        //[HttpPost]
        //[Route("[controller]")]
        //public async Task<IActionResult> RecoverPassword(UserEmail vm)
        //{

        //}
    }
}