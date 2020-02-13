using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult RegisterPage(UserViewModel userV)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                //call api here
            }
            catch
            {
                //send to error?
            }

            return RedirectToPage("./Index");
        }
    }
}