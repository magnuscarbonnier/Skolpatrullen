using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.ViewModels;
using Database.Models;
using Lib;

namespace WebApp.Controllers
{
    public class LoginController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> LoginPage()
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
        public async Task<IActionResult> LoginPage(LoginViewModel loginVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View(new LoginViewModel());
            }
            if (User != null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (loginVM != null)
            {
                APIResponse<LoginSession> response = await APILogin(loginVM);
                if (response.Success)
                {
                    if (response.Data != null)
                    {
                        Response.Cookies.Append("LoginToken", response.Data.Token);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    SetFailureMessage(response.FailureMessage);
                    return View();
                }
            }

            return View();
        }
        [HttpGet]
        [Route("[controller]/Logout")]
        public async Task<IActionResult> Logout()
        {
            string message = await GetUser();
            if (User != null)
            {
                var response = await APILogout(User);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}