using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    public class LessonController : APIController
    {

        public LessonController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpGet]
        [Route("[controller]")]
        public APIResponse<IEnumerable<LessonViewModel>> GetAllLessons()
        {
            APIResponse<IEnumerable<LessonViewModel>> response = new APIResponse<IEnumerable<LessonViewModel>>();
            response.Data = _context.Lessons.Include(l => l.Course).Select(lesson => (LessonViewModel)lesson).ToList();
            response.Success = true;
            response.SuccessMessage = "Hämtade alla lektioner";
            return response;
        }

        [HttpGet]
        [Route("[controller]/{id}")]
        public APIResponse<LessonViewModel> GetLessonById(int id)
        {
            APIResponse<LessonViewModel> response = new APIResponse<LessonViewModel>();
            response.Data = (LessonViewModel)_context.Lessons.Find(id);

            response.Success = true;
            response.SuccessMessage = $"Hämtade alla lektioner för kurs med id {id}";
            return response;
        }

        [HttpPost]
        [Route("[controller]")]
        public ObjectResult AddLesson([FromForm] LessonViewModel lessonVM)
        {
            var newLesson = (Lesson)lessonVM;
            _context.Lessons.Add(newLesson);
            _context.SaveChanges();

            return Ok(new
            {
                action = "Lektion tillagd"
            });
        }

        [HttpPut]
        [Route("[controller]/{id}")]
        public ObjectResult UpdateLesson(int id, [FromForm] LessonViewModel lessonVM)
        {
            var updatedLesson = (Lesson)lessonVM;
            var dbLesson = _context.Lessons.Find(id);
            dbLesson.Name = updatedLesson.Name;
            dbLesson.StartDate = updatedLesson.StartDate;
            dbLesson.EndDate = updatedLesson.EndDate;
            _context.SaveChanges();

            return Ok(new
            {
                action = "Lektion uppdaterad"
            });
        }

        [HttpDelete("{id}")]
        [Route("[controller]/{id}")]
        public ObjectResult DeleteLesson(int id)
        {
            var lesson = _context.Lessons.Find(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                _context.SaveChanges();
            }

            return Ok(new
            {
                action = "Lektion borttagen"
            });
        }
    }
}