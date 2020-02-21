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
        public new User User = null;
        public AppController()
        {
            //det här behövs för att Issas dator är fucked
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient = new HttpClient(clientHandler);
        }

        public async Task<string> GetUser()
        {
            ViewBag.User = null;
            KeyValuePair<string,string>? cookie = Request.Cookies.SingleOrDefault(c => c.Key == "LoginToken");
            if (cookie.Value.Value == null)
            {
                return "Inte inloggad";
            }
            var response = await APIGetLoginSession(cookie.Value.Value);
            if (response.Success)
            {
                User = response.Data.User;
                ViewBag.User = User;
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
        public async Task<HttpResponseMessage> APIGet(string route)
        {
                HttpResponseMessage response = await HttpClient.GetAsync("https://localhost:44367" + route);
                return response;
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
        public async Task<APIResponse<LoginSession>> APIRegister(UserViewModel UserVM)
        {
            HttpResponseMessage response = await APIPost("/User/Register", UserVM);
            return (APIResponse<LoginSession>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<LoginSession>));
        }
        public async Task<APIResponse<bool>> APILogout(User user)
        {
            HttpResponseMessage response = await APIPost("/User/Logout", user);
            return (APIResponse<bool>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<bool>));
        }
        public async Task<APIResponse<IEnumerable<School>>> APIGetAllSchools()
        {
            HttpResponseMessage response = await APIGet("/School/GetAllSchools");
            return (APIResponse<IEnumerable<School>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<School>>));
        }
        public async Task<APIResponse<Room>> APIAddRoom(Room room)
        {
            HttpResponseMessage response = await APIPost("/Room/Add", room);
            return (APIResponse<Room>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<Room>));
        }

        public async Task<APIResponse<User>> APIUpdateUser(User user)
        {
            HttpResponseMessage response = await APIPost("/User/Update", user);
            return (APIResponse<User>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<User>));
        }
    }
}
