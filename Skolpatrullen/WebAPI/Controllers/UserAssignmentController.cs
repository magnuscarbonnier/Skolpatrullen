using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    public class UserAssignmentController : APIController
    {
        public UserAssignmentController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpGet]
        [Route("[controller]/IsReturned/{AssignmentId}/{UserId}")]
        public APIResponse<bool> GetUserAssignment(int AssignmentId, int UserId)
        {
            APIResponse<bool> response = new APIResponse<bool>();
            var isReturned = _context.UserAssignments.SingleOrDefault(ua => ua.AssignmentId == AssignmentId && ua.UserId == UserId && ua.ReturnDate != null);
            if (isReturned != null)
            {
                response.Data = true;
                response.Success = true;
                response.SuccessMessage = $"Elevens inlämning är inlämnad";
            } else
            {
                response.Data = false;
                response.Success = false;
                response.FailureMessage = $"Elevens inlämning är inte inlämnad";
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetAllByUser/{UserId}")]
        public APIResponse<IEnumerable<UserAssignment>> GetAllByUser(int UserId)
        {
            APIResponse<IEnumerable<UserAssignment>> response = new APIResponse<IEnumerable<UserAssignment>>();
            response.Data = _context.UserAssignments.Where(ua => ua.UserId == UserId);

            response.Success = true;
            response.SuccessMessage = $"Hämtade alla inlämningar till elev id: {UserId}";
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetAllByAssignmentId/{assignmentId}")]
        public APIResponse<IEnumerable<UserAssignment>> GetAllByAssignmentId(int assignmentId)
        {
            APIResponse<IEnumerable<UserAssignment>> response = new APIResponse<IEnumerable<UserAssignment>>();
            response.Data = _context.UserAssignments.Where(ua => ua.AssignmentId == assignmentId);

            response.Success = true;
            response.SuccessMessage = $"Hämtade alla inlämningar till uppgift med id: {assignmentId}";
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetAll")]
        public APIResponse<IEnumerable<UserAssignment>> GetAll()
        {
            APIResponse<IEnumerable<UserAssignment>> response = new APIResponse<IEnumerable<UserAssignment>>();
            response.Data = _context.UserAssignments.Select(ua => ua);

            response.Success = true;
            response.SuccessMessage = $"Hämtade alla studentinlämningar";
            return response;
        }
        [HttpPost]
        [Route("[controller]/AddOrUpdate")]
        public APIResponse<UserAssignment> AddOrUpdate(UserAssignment userAssignment)
        {
            APIResponse<UserAssignment> response = new APIResponse<UserAssignment>();
            var existingUserAssignment = _context.UserAssignments.FirstOrDefault(ua => ua.AssignmentId == userAssignment.AssignmentId && ua.UserId == userAssignment.UserId);
            if(existingUserAssignment != null)
            {
                existingUserAssignment.Grade = userAssignment.Grade;
                _context.UserAssignments.Update(existingUserAssignment);
                _context.SaveChanges();
                response.Data = userAssignment;
                response.Success = true;
                response.SuccessMessage = $"Du har uppdaterat en inlämning";
            } 
            else if (userAssignment != null)
            {
                _context.UserAssignments.Add(userAssignment);
                _context.SaveChanges();
                response.Data = userAssignment;
                response.Success = true;
                response.SuccessMessage = $"Du har lämnat in en inlämning";
            }
            else
            {
                response.FailureMessage = $"Det gick inte lämna in inlämning";
                response.Success = false;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/Remove/{UserAssignmentId}")]
        public APIResponse Remove(int UserAssignmentId)
        {
            APIResponse response = new APIResponse();
            var userassignment = _context.Courses.Where(c => c.Id == UserAssignmentId);
            if (userassignment != null)
            {
                _context.Remove(userassignment);
                _context.SaveChanges();
                response.Success = true;
                response.SuccessMessage = $"Tog bort en elev inlämning med id {userassignment}";
            }
            else
            {
                response.FailureMessage = $"Inlämningen fanns ej";
                response.Success = false;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetByCourseUserAndAssignment/{CourseId}/{UserId}/{AssignmentId}")]
        public APIResponse<UserAssignment> GetByCourseUserAndAssignment(int CourseId, int UserId, int AssignmentId)
        {
            APIResponse<UserAssignment> response = new APIResponse<UserAssignment>();
            var userAssignment = _context.UserAssignments.Include(ua => ua.Assignment).Where(ua => ua.Assignment.CourseId == CourseId && ua.AssignmentId == AssignmentId).FirstOrDefault(ua => ua.UserId == UserId);
            if (userAssignment != null)
            {
                response.Data = userAssignment;
                response.Success = true;
                response.SuccessMessage = $"Hämtade inlämning med id {userAssignment.Id}";
            }
            else
            {
                response.Success = false;
                response.FailureMessage = $"Fanns ingen inlämning";
            }

            return response;
        }
        [HttpGet]
        [Route("[controller]/GetUserAssignmenetById/{UserAssignmentId}")]
        public APIResponse<UserAssignment> GetUserAssignmenetById(int id)
        {
            APIResponse<UserAssignment> response = new APIResponse<UserAssignment>();
            var userAssignment = _context.UserAssignments.SingleOrDefault(l => l.Id == id);
            if (userAssignment != null)
            {
                response.Data = userAssignment;
                response.Success = true;
                response.SuccessMessage = $"Hämtade elevs inlämning med id {id}";
            }
            return response;
        }
    }
}
