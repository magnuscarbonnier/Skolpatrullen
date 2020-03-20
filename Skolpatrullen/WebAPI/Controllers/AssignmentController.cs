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
    public class AssignmentController : APIController
    {
        public AssignmentController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
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
                response.SuccessMessage = $"La till kurs med namn {assignment.Name}";
            }
            else
            {
                response.FailureMessage = "Gick inte att lägga till en inlämning";
                response.Success = false;
            }
            return response;
        }
    }
}