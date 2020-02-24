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
            if (User != null)
            {
                var model = new SchoolViewModel();
                var response = await APIGetAllSchools();
                if (response.Data != null)
                {
                    model.SchoolList = response.Data;
                }
                return View(model);

            }
            return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> AddSchoolPage(SchoolViewModel school)
        {
            if (!ModelState.IsValid)
            {
                return View(school);
            }
            try
            {
                var response = await APIAddSchool(school.ToSchool());
                return RedirectToAction("AddSchoolPage", "School");
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("AddSchoolPage", "School");
        }

        [HttpGet]
        [Route("[controller]/Remove/{id}")]
        public async Task<IActionResult> RemoveSchoolPage(SchoolViewModel schoolVM, int id)
        {
            try
            {
                schoolVM.Id = id;
                var response = await APIRemoveSchool(schoolVM.ToSchool());
                return RedirectToAction("SchoolListPage", "School");
            }
            catch
            {
                //send to error?
            }
            return RedirectToAction("Index", "Home");
        }

    }
}