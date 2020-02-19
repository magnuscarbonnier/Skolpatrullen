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
            model.User = User;
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
            try
            {
                var response = await APIAddRoom(roomVM.ToRoom());
                return RedirectToAction("AddRoomPage", "Room");
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("AddRoomPage", "Room");
        }
    }
}