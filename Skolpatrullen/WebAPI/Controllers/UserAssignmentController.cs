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
            var isReturned = _context.UserAssignments.SingleOrDefault(ua => ua.AssignmentId == AssignmentId && ua.UserId == UserId);
            if (isReturned != null)
            {
                response.Data = true;
                response.Success = true;
                response.SuccessMessage = $"Elevens inlämning är inlämnad";
            } else
            {
                response.Data = false;
                response.Success = false;
                response.SuccessMessage = $"Elevens inlämning är inte inlämnad";
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
        [Route("[controller]/GetAll")]
        public APIResponse<IEnumerable<UserAssignment>> GetAll()
        {
            APIResponse<IEnumerable<UserAssignment>> response = new APIResponse<IEnumerable<UserAssignment>>();
            response.Data = _context.UserAssignments.Select(ua => ua);

            response.Success = true;
            response.SuccessMessage = $"Hämtade alla student inlämningar";
            return response;
        }
        [HttpPost]
        [Route("[controller]/AddOrUpdate")]
        public APIResponse<UserAssignment> AddOrUpdate(UserAssignment userAssignment)
        {
            APIResponse<UserAssignment> response = new APIResponse<UserAssignment>();
            var existingUserAssignment = _context.UserAssignments.FirstOrDefault(ua => ua.AssignmentId == userAssignment.AssignmentId);
            if(existingUserAssignment != null)
            {
                existingUserAssignment.Description = userAssignment.Description;
                userAssignment.ReturnDate = existingUserAssignment.ReturnDate;
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
    }
}