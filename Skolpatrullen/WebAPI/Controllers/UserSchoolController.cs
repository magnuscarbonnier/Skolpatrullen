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
    public class UserSchoolController : APIController
    {
        public UserSchoolController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpPost]
        [Route("[controller]/AddOrUpdate")]
        public APIResponse<UserSchool> AddOrUpdate(UserSchool userSchool)
        {
            APIResponse<UserSchool> response = new APIResponse<UserSchool>();
            response.Data = AddOrUpdateUserSchool(userSchool);
            response.Success = true;
            response.SuccessMessage = "La till skoldeltagande";
            return response;
        }
        UserSchool AddOrUpdateUserSchool(UserSchool userSchool)
        {
            var existingUserSchool = _context.UserSchools.SingleOrDefault(us => us.SchoolId == userSchool.SchoolId && us.UserId == userSchool.UserId);
            UserSchool newUserSchool = userSchool;
            if (existingUserSchool != null)
            {
                existingUserSchool.IsAdmin = userSchool.IsAdmin;
                newUserSchool = UpdateUserSchool(existingUserSchool);
            }
            else
            {
                newUserSchool = AddUserSchool(userSchool);
            }
            return newUserSchool;
        }
        UserSchool AddUserSchool(UserSchool userSchool)
        {
            _context.UserSchools.Add(userSchool);
            _context.SaveChanges();
            return userSchool;
        }
        UserSchool UpdateUserSchool(UserSchool userSchool)
        {
            _context.UserSchools.Update(userSchool);
            _context.SaveChanges();
            return userSchool;
        }
    }
}