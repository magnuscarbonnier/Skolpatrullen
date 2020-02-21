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
    public class AdminController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILogger<AdminController> _logger;
        public AdminController(Context context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost]
        [Route("[controller]/Add")]
        public APIResponse<UserSchool> Add(UserSchool userSchool)
         {
            APIResponse<UserSchool> response = new APIResponse<UserSchool>();
            if (userSchool != null)
            {
                _context.UserSchools.Add(userSchool);
                _context.SaveChanges();
                response.Success = true;
                response.Data = userSchool;
            }
            return response;
        }
    }
}