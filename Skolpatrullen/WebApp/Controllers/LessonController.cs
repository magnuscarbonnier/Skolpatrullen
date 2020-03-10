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
        public async Task<IActionResult> LessonCalendar()
        {
            string message = await GetUser();
            if (User != null)
            {
                var model = new LessonViewModel();
                IEnumerable<LessonViewModel> lessonList = new List<LessonViewModel>();
                var response = await APIGetAllLessons();
                if (response.Data != null)
                {
                    lessonList = response.Data;
                }
                return View("LessonList", lessonList);

            }
            return RedirectToAction("Index", "Home");
        }
    }
}