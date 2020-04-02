using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class AssignmentController : APIController
    {
        public AssignmentController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }
        
        [HttpGet]
        [Route("[controller]/GetAssignmentByCourse/{CourseId}")]
        public APIResponse<IEnumerable<Assignment>> GetAssignmentByCourse(int CourseId)
        {
            APIResponse<IEnumerable<Assignment>> response = new APIResponse<IEnumerable<Assignment>>();
            response.Data = _context.Assignments.Where(a => a.CourseId == CourseId);
            if (response.Data != null)
            {
                response.Success = true;
                response.SuccessMessage = $"Hämtade inlämningar med kurs id {CourseId}";
            }
            else
            {
                response.FailureMessage = "Gick inte att hämta inlämningar!";
                response.Success = false;
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public APIResponse<Assignment> Add(Assignment assignment)
        {
            APIResponse<Assignment> response = new APIResponse<Assignment>();
            if (assignment != null)
            {
                _context.Assignments.Add(assignment);
                _context.SaveChanges();
                response.Data = assignment;
                response.Success = true;
                response.SuccessMessage = $"La till inlämning med namn {assignment.Name}";
            }
            else
            {
                response.FailureMessage = "Gick inte att lägga till en inlämning";
                response.Success = false;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetAssignmentById/{AssignmentId}")]
        public APIResponse<Assignment> GetAssignmentById(int AssignmentId)
        {
            APIResponse<Assignment> response = new APIResponse<Assignment>();
            response.Data = _context.Assignments.SingleOrDefault(a => a.Id == AssignmentId);

            response.Success = true;
            response.SuccessMessage = $"Hämtade inlämning med id {AssignmentId}";
            return response;
        }
    }
}
