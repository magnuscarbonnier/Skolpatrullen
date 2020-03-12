using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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
                if (SendPasswordRecoveryMail(user.Email, recovery.Token))
                {
                    response.Success = true;
                    response.Data = recovery;
                    response.SuccessMessage = "Skickade återställningsmejl till " + email;
                }
                else
                {
                    response.Success = false;
                    response.FailureMessage = "Kunde inte skicka återställningsmejl till " + email;
                }
            }
            else
            {
                response.Success = false;
                response.FailureMessage = "Det finns ingen användare med mejladdressen " + email;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetByToken/{token}")]
        public APIResponse<PasswordRecovery> GetByToken(string token)
        {
            var response = new APIResponse<PasswordRecovery>();
            var recovery = _context.PasswordRecoveries.Include(pr => pr.User).SingleOrDefault(pr => pr.Token == token);
            if (recovery != null)
            {
                if (recovery.ExpireTime >= DateTime.Now)
                {
                    response.Data = recovery;
                    response.Success = true;
                    response.SuccessMessage = "Hämtade PasswordRecovery med token " + token;
                }
                else
                {
                    response.Success = false;
                    response.FailureMessage = "Tiden har gått ut på PasswordRecovery med token " + token;
                }
            }
            else
            {
                response.Success = false;
                response.FailureMessage = "Hittade ingen PasswordRecovery med token " + token;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/DeleteByToken/{token}")]
        public APIResponse<PasswordRecovery> DeleteByToken(string token)
        {
            var response = new APIResponse<PasswordRecovery>();
            var recovery = _context.PasswordRecoveries.SingleOrDefault(pr => pr.Token == token);
            if (recovery != null)
            {
                _context.PasswordRecoveries.Remove(recovery);
                _context.SaveChanges();
                response.Data = recovery;
                response.Success = true;
                response.SuccessMessage = "Tog bort PasswordRecovery med token " + token;
            }
            else
            {
                response.Success = true;
                response.SuccessMessage = $"PasswordRecovery med token {token} är redan borttagen/finns inte";
            }
            return response;
        }
        bool SendPasswordRecoveryMail(string email, string token)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("send.one.com");

            mail.From = new MailAddress("skolpatrullen@kittycatcrew.se");
            mail.To.Add(email);
            mail.Subject = "Skolpatrullen: Återställ lösenord";
            mail.Body = "https://localhost:44382/PasswordRecovery/NewPassword/" + token;

            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("skolpatrullen@kittycatcrew.se", "IssaharCorona");
            SmtpServer.EnableSsl = true;
            try
            {
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}