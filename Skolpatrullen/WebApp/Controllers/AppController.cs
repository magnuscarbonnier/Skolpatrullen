using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAPI;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public abstract class AppController : Controller
    {
        private readonly HttpClient HttpClient;
        public User User = null;
        public AppController()
        {
            HttpClient = new HttpClient();
        }

        public async void GetUser()
        {
            KeyValuePair<string,string>? cookie = Request.Cookies.SingleOrDefault(c => c.Key == "LoginToken");
            if (cookie.Value.Value == null)
            {
                throw new Exception("Inte inloggad");
            }
            var response = await APIGetLoginSession(cookie.Value.Value);
            if (response.Success)
            {
                User = response.Data.User;
            }
            else
            {
                string errorMessage = "";
                foreach (string em in response.ErrorMessages)
                {
                    errorMessage += em + "\n";
                }
                throw new Exception(errorMessage);
            }
        }

        public async Task<HttpResponseMessage> APIPost<T>(string route, T body)
        {
            var json = JsonConvert.SerializeObject(body);
            using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await HttpClient.PostAsync("https://localhost:44367"+route, stringContent);
                return response;
            }
        }
        public async Task<APIResponse<bool>> APILogin(LoginViewModel loginVM)
        {
            HttpResponseMessage response = await APIPost("/User/Login", loginVM);
            return (APIResponse<bool>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
        }
        public async Task<APIResponse<LoginSession>> APIGetLoginSession(string token)
        {
            HttpResponseMessage response = await APIPost("/User/GetLoginSession", token);
            return (APIResponse<LoginSession>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
        }
    }
}
