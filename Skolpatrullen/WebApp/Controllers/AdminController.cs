using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AdminController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> AddAdminPage()
        {
            string message = await GetUser();
            var model = new AdminViewModel();
            model.User = User;
            var response = await APIGetAllSchools();
            if (response.Data != null)
            {
                model.SchoolList = response.Data;
            }
            //var response2 = await APIGetAllUsers();
            //if (response.Data != null)
            //{
            //    model.UserList = response2.Data;
            //}
            return View(model);
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> AddAdminPage(AdminViewModel adminVM)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                var response = await APIAddUserSchool(adminVM.ToUserSchool());
                return RedirectToAction("AddAdminPage", "Admin");
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("AddAdminPage", "Admin");
        }
    }
}