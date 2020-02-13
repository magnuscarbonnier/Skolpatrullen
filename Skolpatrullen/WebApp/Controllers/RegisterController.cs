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
    public class RegisterController : Controller
    {
        private readonly HttpClient _client;
        public RegisterController()
        {
            _client = new HttpClient();
        }
        [HttpGet]
        [Route("[controller]")]
        public IActionResult RegisterPage()
        {
            return View();
        }
        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> RegisterPage(UserViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            HttpResponseMessage response;
            try
            {
                if (!(userVM.Password == userVM.RePassword))
                {
                    userVM.Password = "";
                    userVM.RePassword = "";
                    return View(userVM);
                }
                var json = JsonConvert.SerializeObject(userVM.ToUser());
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    response = await _client.PostAsync("https://localhost:44367/User/Register", stringContent);
                }
            }
            catch
            {
                //send to error?
            }
            return View();
        }
    }
}