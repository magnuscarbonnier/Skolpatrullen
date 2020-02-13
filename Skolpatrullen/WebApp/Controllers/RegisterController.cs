using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class RegisterController : Controller
    {
        [HttpGet]
        public IActionResult RegisterPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterPage(UserViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                if (!(userVM.Password == userVM.RePassword))
                {
                    userVM.Password = "";
                    userVM.RePassword = "";
                    return View(userVM);
                }
                User user = userVM.ToUser();
                //call User/Register api
            }
            catch
            {
                //send to error?
            }

            return RedirectToPage("./Index");
        }
    }
}