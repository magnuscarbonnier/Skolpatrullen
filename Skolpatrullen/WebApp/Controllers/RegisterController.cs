using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class RegisterController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> RegisterPage()
        {
            string message = await GetUser();
            if (User != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> RegisterPage(UserViewModel userVM)
        {
            string message = await GetUser();
            if (User != null)
            {
                return RedirectToAction("Index", "Home"); 
            }
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
                    TempData["ErrorMessage"] = $"Lösenordet matchade inte, försök igen.";
                    return View(userVM);
                }
                var response = await APIRegister(userVM);
                if (response.Data != null)
                {
                    Response.Cookies.Append("LoginToken", response.Data.Token);
                    TempData["SuccessMessage"] = $"Användare tillagd.";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                TempData["ErrorMessage"] = $"Error!";
                //send to error?
            }
            
            return View();
        }
    }
}