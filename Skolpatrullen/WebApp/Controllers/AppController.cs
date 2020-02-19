using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<string> GetUser()
        {
            KeyValuePair<string,string>? cookie = Request.Cookies.SingleOrDefault(c => c.Key == "LoginToken");
            if (cookie.Value.Value == null)
            {
                return "Inte inloggad";
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
                return errorMessage;
            }
            return "Något gick fel ¯\\_(ツ)_/¯";
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
        public async Task<APIResponse<LoginSession>> APILogin(LoginViewModel loginVM)
        {
            HttpResponseMessage response = await APIPost("/User/Login", loginVM);
            return (APIResponse<LoginSession>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(),typeof(APIResponse<LoginSession>));
        }
        public async Task<APIResponse<LoginSession>> APIGetLoginSession(string token)
        {
            HttpResponseMessage response = await APIPost("/User/GetLoginSession", new TokenBody { token = token });
            return (APIResponse<LoginSession>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<LoginSession>));
        }
    }
}
