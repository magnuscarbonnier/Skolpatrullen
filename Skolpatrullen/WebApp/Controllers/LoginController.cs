using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI;
using WebApp.Models;
using WebApp.ViewModels;
using Database.Models;

namespace WebApp.Controllers
{
    public class LoginController : AppController
    {
        private readonly HttpClient _client;
        public LoginController()
        {
            _client = new HttpClient();
        }
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> LoginPage()
        {
            try
            {
                await GetUser();
            }
            catch
            {

            }
            if (User == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> LoginPage(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
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
                        //forward error messages in response to view
                        return View();
                    }
                }
            }
            catch
            {

            }
            return View();
        }
    }
}