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
    public class PasswordRecoveryController : APIController
    {
        public PasswordRecoveryController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }
        [HttpGet]
        [Route("[controller]/ByEmail/{email}")]
        public APIResponse<PasswordRecovery> ByEmail(string email)
        {
            var response = new APIResponse<PasswordRecovery>();
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user != null)
            {
                var expired = _context.PasswordRecoveries.Where(pr => pr.ExpireTime <= DateTime.Now);
                _context.RemoveRange(expired);
                var recovery = new PasswordRecovery() { UserId = user.Id, ExpireTime = DateTime.Now.AddMinutes(15) };
                _context.Add(recovery);
                _context.SaveChanges();
                response.Success = true;
                response.Data = recovery;
            }
            else
            {
                response.Success = false;
                response.FailureMessage = "Det finns ingen användare med mejladdressen " + email;
            }
            return response;
        }
    }
}