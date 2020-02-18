using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.ViewModels;

namespace WebAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Context _context;
        private readonly ILogger<UserController> _logger;
        public UserController(Context context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpPost]
        [Route("[controller]/Register")]
        public APIResponse<LoginSession> Register(User user)
        {
            APIResponse<LoginSession> response = new APIResponse<LoginSession>();
            if (!_context.Users.Any(u => u.Email == user.Email))
            {
                user.Password = ComputeSha256Hash(user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                response.Data = AddLoginSession(user);
                response.Success = true;
            }
            else
            {
                response.ErrorMessages.Add($"Det finns redan en användare med mejladressen {user.Email}");
                response.Success = false;
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/Login")]
        public APIResponse<LoginSession> Login(LoginViewModel login)
        {
            APIResponse<LoginSession> response = new APIResponse<LoginSession>();
            User user = _context.Users.SingleOrDefault(u => u.Email == login.Email);
            if (user == null)
            {
                response.ErrorMessages.Add($"Det finns ingen användare med mejladressen {login.Email}");
                response.Success = false;
            }
            else if (user.Password == ComputeSha256Hash(login.Password))
            {
                response.Data = AddOrUpdateLoginSession(user);
                response.Success = true;
            }
            else
            {
                response.ErrorMessages.Add("Fel lösenord");
                response.Success = false;
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/GetLoginSession")]
        public APIResponse<LoginSession> GetLoginSession(TokenBody token)
        {
            APIResponse<LoginSession> response = new APIResponse<LoginSession>();
            LoginSession session = _context.LoginSessions.Include(ls => ls.User).SingleOrDefault(ls => ls.Token == token.token);
            if (session == null)
            {
                response.ErrorMessages.Add($"Hittade ingen LoginSession med token: {token}");
                response.Success = false;
            }
            else if (session.Expires < DateTime.Now.ToUniversalTime())
            {
                response.ErrorMessages.Add($"LoginSession har gått ut");
                response.Success = false;
            }
            else
            {
                UpdateLoginSession(session);
                response.Data = session;
                response.Success = true;
            }
            return response;
        }
        LoginSession AddOrUpdateLoginSession(User user)
        {
            var session = _context.LoginSessions.FirstOrDefault(ls => ls.UserId == user.Id);
            if (session == null)
            {
                session = AddLoginSession(user);
            }
            else
            {
                session = UpdateLoginSession(session);
            }
            return session;
        }
        LoginSession AddLoginSession(User user)
        {
            LoginSession session = new LoginSession()
            {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.Now.AddMinutes(15).ToUniversalTime(),
                UserId = user.Id
            };
            _context.LoginSessions.Add(session);
            _context.SaveChanges();
            return session;
        }
        LoginSession UpdateLoginSession(LoginSession session)
        {
            session.Expires = DateTime.Now.AddMinutes(15).ToUniversalTime();
            _context.LoginSessions.Update(session);
            _context.SaveChanges();
            return session;
        }
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public class TokenBody
        {
            public string token { get; set; }
        }
    }
}