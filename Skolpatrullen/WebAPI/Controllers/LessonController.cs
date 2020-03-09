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
        [Route("[controller]/GetAllLessons")]
        public APIResponse<IEnumerable<LessonViewModel>> GetAllLessons()
        {
            APIResponse<IEnumerable<LessonViewModel>> response = new APIResponse<IEnumerable<LessonViewModel>>();
            response.Data = _context.Lessons.ToList().Select(lesson => (LessonViewModel)lesson);
            response.Success = true;
            response.SuccessMessage = "Hämtade alla lektioner";
            return response;
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

        [HttpGet]
        [Route("[controller]/GetLessonById/{id}")]
        public APIResponse<LessonViewModel> GetLessonById(int id)
        {
            APIResponse<LessonViewModel> response = new APIResponse<LessonViewModel>();
            response.Data = (LessonViewModel)_context.Lessons.Find(id);

            response.Success = true;
            response.SuccessMessage = $"Hämtade alla lektioner för kurs med id {id}";
            return response;
        }
    }
}