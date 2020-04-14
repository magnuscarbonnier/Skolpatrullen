using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class SchoolController : AppController
    {
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> SchoolListPage()
        {
            string message = await GetUser();
            var model = new SchoolViewModel();
            var response = await APIGetAllSchools();
            if (response.Data != null)
            {
                model.SchoolList = response.Data;
            }
            return View(model);
        }

        [HttpGet]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddSchoolPage()
        {
            string message = await GetUser();
            if (User != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("[controller]/Add")]
        public async Task<IActionResult> AddSchool(SchoolViewModel school)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View(school);
            }
            var response = await APIAddSchool(school.ToSchool());
            SetResponseMessage(response);
            return RedirectToAction("AddSchoolPage", "School");
        }

        [HttpGet]
        [Route("[controller]/Remove/{id}")]
        public async Task<IActionResult> RemoveSchoolPage(int id)
        {
            string message = await GetUser();
            var response = await APIRemoveSchool(id);
            return SetResponseMessage(response, RedirectToAction("SchoolListPage", "School"), RedirectToAction("Index", "Home"));
        }

    }
}