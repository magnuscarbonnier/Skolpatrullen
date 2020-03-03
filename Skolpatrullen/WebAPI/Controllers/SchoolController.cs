using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILogger<SchoolController> _logger;
        public SchoolController(Context context, ILogger<SchoolController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[controller]/GetAllSchools")]
        public APIResponse<IEnumerable<School>> GetAllSchools()
        {
            APIResponse<IEnumerable<School>> response = new APIResponse<IEnumerable<School>>();
            var schoollist = _context.Schools.OrderBy(s => s.Name).ToList();
            if (schoollist != null)
            {
                response.Success = true;
                response.Data = schoollist;
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
                response.Success = true;
                response.Data = school;
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
        public APIResponse<bool> Remove(int id)
        {
            APIResponse<bool> response = new APIResponse<bool>();
            var removeschool = _context.Schools.SingleOrDefault(s => s.Id == id);
            if (removeschool != null)
            {
                _context.Remove(removeschool);
                _context.SaveChanges();
                response.Success = true;
            }
            else
            {
                response.FailureMessage = $"Skolan fanns inte";
                response.Success = false;
            }
            return response;
        }
    }
}