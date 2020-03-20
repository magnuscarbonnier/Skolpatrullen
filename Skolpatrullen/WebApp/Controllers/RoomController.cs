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
        public async Task<IActionResult> AddRoomPage()
        {
            string message = await GetUser();
            var model = new RoomViewModel();
            var response = await APIGetAllSchools();
            if (response.Data != null)
            {
                model.SchoolList = response.Data;
            }
            return View(model);
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> AddRoomPage(RoomViewModel roomVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await APIAddRoom(roomVM.ToRoom());
            SetResponseMessage(response);
            return RedirectToAction("AddRoomPage", "Room");
        }
    }
}