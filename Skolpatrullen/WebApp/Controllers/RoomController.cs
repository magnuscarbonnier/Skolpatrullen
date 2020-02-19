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
    public class RoomController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public IActionResult AddRoomPage()
        {
            var model = new RoomViewModel();
            model.SchoolList = GetAllSchools();
            return View(model);
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> AddRoomPage(RoomViewModel roomVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            HttpResponseMessage response;
            try
            {
                var json = JsonConvert.SerializeObject(roomVM.ToRoom());
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    response = await _client.PostAsync("https://localhost:44367/Room/Add", stringContent);
                }
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                //send to error?
            }
            return View();
        }

        public IEnumerable<School> GetAllSchools()
        {
            //HttpResponseMessage response;
            _client.DefaultRequestHeaders.Add("Get", "application/json");
            var json = _client.GetAsync("https://localhost:44367/School/GetAllSchools").Result;

            string schools = json.Content.ReadAsStringAsync().Result;
            SchoolList = JsonConvert.DeserializeObject<IEnumerable<School>>(schools);
            return SchoolList;
        }
    }
}