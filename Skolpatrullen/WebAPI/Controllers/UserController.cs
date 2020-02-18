using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
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
                //create login session for new user
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
        public APIResponse<bool> Login(LoginViewModel login)
        {
            APIResponse<bool> response = new APIResponse<bool>();
            User user = _context.Users.Where(u => u.Email == login.Email).FirstOrDefault();
            if (user == null)
            {
                response.Data = false;
                response.ErrorMessages.Add($"Det finns ingen användare med mejladressen {login.Email}");
                response.Success = false;
            }
            else if (user.Password == ComputeSha256Hash(login.Password))
            {
                response.Data = true;
                response.Success = true;
            }
            return response;
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
    }
}