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
            var lessonlist = _context.Lessons.Include(l => l.Course).Select(lesson => (LessonViewModel)lesson).ToList();
            if(lessonlist != null)
            {
                response.Data = lessonlist;
                response.Success = true;
                response.SuccessMessage = "Hämtade alla lektioner";
            }
            return response;
        }

        [HttpGet]
        [Route("[controller]/{id}")]
        public APIResponse<LessonViewModel> GetLessonById(int id)
        {
            APIResponse<LessonViewModel> response = new APIResponse<LessonViewModel>();
            var lesson = (LessonViewModel)_context.Lessons.SingleOrDefault(l => l.Id == id);
            if(lesson != null)
            {
                response.Data = lesson;
                response.Success = true;
                response.SuccessMessage = $"Hämtade lektionen med id {id}";
            }
            return response;
        }

        [HttpPost]
        [Route("[controller]")]
        public APIResponse<Lesson> AddLesson([FromForm] LessonViewModel lessonVM)
        {
            APIResponse<Lesson> response = new APIResponse<Lesson>();
            var newLesson = (Lesson)lessonVM;
            if(newLesson != null)
            {
                _context.Lessons.Add(newLesson);
                _context.SaveChanges();
                response.Data = newLesson;
                response.Success = true;
                response.SuccessMessage = $"Lektionen med id: {lessonVM.id} tillagd";
            } else
            {
                response.FailureMessage = $"Lektionen fanns inte";
                response.Success = false;
            }
            return response;
        }

        [HttpPut]
        [Route("[controller]/{id}")]
        public APIResponse<Lesson> UpdateLesson(int id, [FromForm] LessonViewModel lessonVM)
        {
            APIResponse<Lesson> response = new APIResponse<Lesson>();
            var updatedLesson = (Lesson)lessonVM;
            var dbLesson = _context.Lessons.SingleOrDefault(l => l.Id == id);
            if(dbLesson != null)
            {
                dbLesson.Name = updatedLesson.Name;
                dbLesson.StartDate = updatedLesson.StartDate;
                dbLesson.EndDate = updatedLesson.EndDate;
                _context.SaveChanges();
                response.Data = updatedLesson;
                response.Success = true;
                response.SuccessMessage = $"Lektion {id} uppdaterad";
            } else
            {
                response.FailureMessage = $"Lektionen {id} fanns ej";
                response.Success = false;
            }
            return response;
        }

        [HttpDelete("{id}")]
        [Route("[controller]/{id}")]
        public APIResponse<Lesson> DeleteLesson(int id)
        {
            APIResponse<Lesson> response = new APIResponse<Lesson>();
            var lesson = _context.Lessons.SingleOrDefault(l => l.Id == id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                _context.SaveChanges();
                response.Success = true;
                response.SuccessMessage = $"Lektion {id} borttagen";
            } else
            {
                response.FailureMessage = $"Lektionen med id:{id} kunde ej tas bort";
                response.Success = false;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/UserLessons/{UserId}")]
        public APIResponse<IEnumerable<LessonViewModel>> GetLessonsByUserId(int UserId)
        {
            APIResponse<IEnumerable<LessonViewModel>> response = new APIResponse<IEnumerable<LessonViewModel>>();
            var lessonlist = _context.Lessons
                .Include(les => les.Course)
                    .ThenInclude(c => c.CourseParticipants)
                .Where(les => les.Course.CourseParticipants.Any(cp => cp.UserId == UserId && cp.Status == Status.Antagen))
                .Select(les => (LessonViewModel)les)
                .ToList();
            
            if (lessonlist != null)
            {
                response.Data = lessonlist;
                response.Success = true;
                response.SuccessMessage = "Hämtade alla lektioner";
            }
            return response;
        }
    }
}