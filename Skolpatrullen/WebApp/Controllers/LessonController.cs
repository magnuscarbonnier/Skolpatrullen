using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class LessonController : AppController
    {
        [HttpGet]
        [Route("[controller]/Calendar")]
        public async Task<IActionResult> LessonCalendar(int id)
        {
            string message = await GetUser();
            if (User != null)
            {
                var course = (await APIGetCourseById(id)).Data;
                return View("LessonList", course);

            }
            return RedirectToAction("Index", "Home");
        }
    }
}