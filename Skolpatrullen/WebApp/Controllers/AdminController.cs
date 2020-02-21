﻿using System;
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
            var schoolResponse = await APIGetAllSchools();
            if (schoolResponse.Data != null)
            {
                model.SchoolList = schoolResponse.Data;
            }
            var userResponse = await APIGetAllUsers();
            if (schoolResponse.Data != null)
            {
                model.UserList = userResponse.Data;
            }
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
                var admin = adminVM.ToUserSchool();
                admin.IsAdmin = true;
                var response = await APIAddOrUpdateUserSchool(admin);
                TempData["SuccessMessage"] = $"Admin tillagd.";
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