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

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _client;
        public LoginController()
        {
            _client = new HttpClient();
        }
        [HttpGet]
        [Route("[controller]")]
        public IActionResult LoginPage()
        {
            return View();
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
                    var json = JsonConvert.SerializeObject(loginVM);
                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = await _client.PostAsync("https://localhost:44367/User/Login", stringContent);
                        if (bool.Parse(await response.Content.ReadAsStringAsync()))
                        {
                            return RedirectToAction("Index", "Home");
                        }
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