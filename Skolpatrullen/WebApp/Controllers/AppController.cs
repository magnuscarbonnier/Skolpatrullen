using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public abstract class AppController : Controller
    {
        private readonly HttpClient HttpClient;
        private readonly User User;
        public AppController()
        {
            HttpClient = new HttpClient();
        }

        public void GetUser()
        {
            KeyValuePair<string,string>? cookie = Request.Cookies.SingleOrDefault(c => c.Key == "LoginToken");
            if (cookie.Value.Value == null)
            {
                throw new Exception("Inte inloggad");
            }

        }

        public async Task<HttpResponseMessage> APIPost<T>(string route, T body)
        {
            var json = JsonConvert.SerializeObject(body);
            using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await HttpClient.PostAsync("https://localhost:44367/User/Login", stringContent);
                return response;
            }
        }
    }
}
