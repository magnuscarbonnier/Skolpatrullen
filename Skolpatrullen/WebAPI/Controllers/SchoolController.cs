using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class SchoolController : APIController
    {
        public SchoolController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpGet]
        [Route("[controller]/GetAllSchools")]
        public APIResponse<IEnumerable<School>> GetAllSchools()
        {
            APIResponse<IEnumerable<School>> response = new APIResponse<IEnumerable<School>>();
            var schoollist = _context.Schools.OrderBy(s => s.Name).ToList();
            if (schoollist != null)
            {
                response.Data = schoollist;
                response.Success = true;
                response.SuccessMessage = "Hämtade alla skolor";
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetSchoolsByUserId/{Id}")]
        public APIResponse<IEnumerable<School>> GetSchoolsByUserId(int Id)
        {
            APIResponse<IEnumerable<School>> response = new APIResponse<IEnumerable<School>>();
            var schools = _context.CourseParticipants
                  .Include(cp => cp.Course)
                  .ThenInclude(cp => cp.School)
                  .Where(cp => cp.UserId == Id)
                  .Select(cp => new School
                  {
                      Id = cp.Course.School.Id,
                      Address = cp.Course.School.Address,
                      Name = cp.Course.School.Name,
                      Phone = cp.Course.School.Phone,
                      PostalCode = cp.Course.School.PostalCode,
                      City = cp.Course.School.City
                  })
                  .Distinct();
            if (schools.Any())
            {
                response.Data = schools;
                response.Success = true;
                response.SuccessMessage = $"Hämtade alla skolor för användare med id {Id}";
            }
            else
            {
                response.Success = false;
                response.FailureMessage = $"Fanns inga skolor för användare med id {Id}";
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/AddSchool")]
        public APIResponse<School> Add(School school)
        {
            APIResponse<School> response = new APIResponse<School>();
            if (!_context.Schools.Any(s => s.Name == school.Name))
            {
                _context.Schools.Add(school);
                _context.SaveChanges();
                response.Data = school;
                response.Success = true;
                response.SuccessMessage = $"La till skola med namn {school.Name}";
            }
            else
            {
                response.FailureMessage = $"Det finns redan en skola med det namnet";
                response.Success = false;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/RemoveSchool/{id}")]
        public APIResponse Remove(int id)
        {
            APIResponse response = new APIResponse();
            var removeschool = _context.Schools.SingleOrDefault(s => s.Id == id);
            if (removeschool != null)
            {
                _context.Remove(removeschool);
                _context.SaveChanges();
                response.Success = true;
                response.SuccessMessage = $"Tog bort skola med id {id}";
            }
            else
            {
                response.FailureMessage = $"Skolan finns inte";
                response.Success = false;
            }
            return response;
        }
    }
}