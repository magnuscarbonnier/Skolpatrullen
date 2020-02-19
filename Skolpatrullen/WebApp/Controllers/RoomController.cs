using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class RoomController : Controller
    {
        private readonly HttpClient _client;

        public RoomController()
        {
            _client = new HttpClient();
        }

        [HttpGet]
        [Route("[controller]")]
        public IActionResult AddRoomPage()
        {
            return View();
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
            }
            catch
            {
                //send to error?
            }
            return View();
        }
    }
}