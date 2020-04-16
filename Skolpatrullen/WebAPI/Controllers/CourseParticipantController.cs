using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class CourseParticipantController : APIController
    {
        public CourseParticipantController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpGet]
        [Route("[controller]/GetCourseParticipantsNotYetAccepted")]
        public APIResponse<int> GetCourseParticipantsNotYetAccepted()
        {
            var response = new APIResponse<int>();
            var count = _context.CourseParticipants.Where(cp=>cp.Status==Status.Ansökt);
            if (count != null)
            {
                response.Data = count.Count();
                response.Success = true;
            }
            return response;
        }

        [HttpGet]
        [Route("[controller]/GetAll")]
        public APIResponse<IEnumerable<CourseParticipant>> GetAll()
        {
            APIResponse<IEnumerable<CourseParticipant>> response = new APIResponse<IEnumerable<CourseParticipant>>();
            var courseParticipants = _context.CourseParticipants.ToList();
            if (courseParticipants != null)
            {
                response.Data = courseParticipants;
                response.Success = true;
                response.SuccessMessage = "Hämtade alla kursdeltaganden";
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/AddOrUpdate")]
        public APIResponse<CourseParticipant> AddOrUpdate(CourseParticipant courseParticipant)
        {
            APIResponse<CourseParticipant> response = new APIResponse<CourseParticipant>();
            response.Data = AddOrUpdateCourseParticipant(courseParticipant);
            response.Success = true;
            response.SuccessMessage = "La till/uppdaterade kursdeltagande";
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetCourseParticipantsByUserId/{Id}")]
        public APIResponse<IEnumerable<CourseParticipant>> GetCourseParticipantsByUserId(int Id)
        {
            APIResponse<IEnumerable<CourseParticipant>> response = new APIResponse<IEnumerable<CourseParticipant>>();

            var courseParticipants = _context.CourseParticipants.Where(cp => cp.UserId == Id);

            if (courseParticipants.Any())
            {
                response.Data = courseParticipants;
                response.Success = true;
                response.SuccessMessage = $"Hämtade alla kursdeltaganden för användare med id {Id}";
            }
            else
            {
                response.Success = false;
                response.FailureMessage = $"Fanns inga kursdeltaganden för användare med id {Id}";
            }

            return response;
        }
        [HttpGet]
        [Route("[controller]/GetCourseParticipantById/{Id}")]
        public APIResponse<CourseParticipant> GetCourseParticipantById(int Id)
        {
            APIResponse<CourseParticipant> response = new APIResponse<CourseParticipant>();
            response.Data = _context.CourseParticipants.SingleOrDefault(cp => cp.Id == Id);
            if (response.Data != null)
            {
                response.Success = true;
                response.SuccessMessage = "Hämtade kursdeltagande med id " + Id;
            }
            else
            {
                response.Success = false;
                response.FailureMessage = "Kunde inte hitta kursdeltagande med id " + Id;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/Remove/{id}")]
        public APIResponse Remove(int id)
        {
            APIResponse response = new APIResponse();
            var removeCP = _context.CourseParticipants.SingleOrDefault(s => s.Id == id);
            if (removeCP != null && removeCP.Status == Status.Ansökt && removeCP.Grade == null)
            {
                _context.Remove(removeCP);
                _context.SaveChanges();
                response.Success = true;
                response.SuccessMessage = $"Tog bort kursdeltagare med id {id}";
            }
            else if (removeCP != null && removeCP.Status != Status.Ansökt)
            {
                response.Success = false;
                response.SuccessMessage = $"Du får ej ta bort kursdeltagare med id {id}. Går endast att ta bort om status är Ansökt och betyg ej är satt.";
            }
            else
            {
                response.FailureMessage = $"Kursdeltagaren fanns inte";
                response.Success = false;
            }
            return response;
        }
        CourseParticipant AddOrUpdateCourseParticipant(CourseParticipant courseParticipant)
        {
            var existingCourseParticipant = _context.CourseParticipants.SingleOrDefault(cp => cp.CourseId == courseParticipant.CourseId && cp.UserId == courseParticipant.UserId);
            CourseParticipant newCourseParticipant = courseParticipant;
            if (existingCourseParticipant != null)
            {
                existingCourseParticipant.Grade = courseParticipant.Grade;
                existingCourseParticipant.Role = courseParticipant.Role;
                existingCourseParticipant.Status = courseParticipant.Status;
                newCourseParticipant = UpdateCourseParticipant(existingCourseParticipant);
            }
            else
            {
                newCourseParticipant = AddCourseParticipant(courseParticipant);
            }
            return newCourseParticipant;
        }
        CourseParticipant AddCourseParticipant(CourseParticipant courseParticipant)
        {
            courseParticipant.ApplicationDate = DateTime.Now;
            _context.CourseParticipants.Add(courseParticipant);
            _context.SaveChanges();
            return courseParticipant;
        }
        CourseParticipant UpdateCourseParticipant(CourseParticipant courseParticipant)
        {
            _context.CourseParticipants.Update(courseParticipant);
            _context.SaveChanges();
            return courseParticipant;
        }
    }
}
