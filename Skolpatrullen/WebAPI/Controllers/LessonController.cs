using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.ViewModels;

namespace WebAPI.Controllers
{
    [ApiController]
    public class LessonController : APIController
    {
        public LessonController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpGet]
        [Route("[controller]/Lessons")]
        public APIResponse<IEnumerable<LessonViewModel>> GetAllLessons()
        {
            APIResponse<IEnumerable<LessonViewModel>> response = new APIResponse<IEnumerable<LessonViewModel>>();
            response.Data = _context.Lessons.ToList().Select(lesson => (LessonViewModel)lesson);
            response.Success = true;
            response.SuccessMessage = "Hämtade alla lektioner";
            return response;
        }

        [HttpGet]
        [Route("[controller]/Lessons/{id}")]
        public APIResponse<LessonViewModel> GetLessonById(int id)
        {
            APIResponse<LessonViewModel> response = new APIResponse<LessonViewModel>();
            response.Data = (LessonViewModel)_context.Lessons.Find(id);

            response.Success = true;
            response.SuccessMessage = $"Hämtade alla lektioner för kurs med id {id}";
            return response;
        }

        [HttpPost]
        [Route("[controller]/Lessons")]
        public ObjectResult Post([FromForm] LessonViewModel lessonVM)
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
        [Route("[controller]/Lessons/{id}")]
        public ObjectResult Put(int id, [FromForm] LessonViewModel lessonVM)
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
        [Route("[controller]/Lessons/{id}")]
        public ObjectResult DeleteEvent(int id)
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

        [HttpGet]
        [Route("[controller]/GetLessonByCourseId/{id}")]
        public APIResponse<IEnumerable<LessonViewModel>> GetLessonsByCourseId(int id)
        {
            APIResponse<IEnumerable<LessonViewModel>> response = new APIResponse<IEnumerable<LessonViewModel>>();
            response.Data = _context.Lessons.ToList().Where(lesson => lesson.Course.Id == id).Select(lesson => (LessonViewModel)lesson);

            response.Success = true;
            response.SuccessMessage = $"Hämtade alla lektioner för kurs med id {id}";
            return response;
        }
    }
}