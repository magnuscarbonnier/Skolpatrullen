using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public ActionResult<User> Register(User user)
        {
            if (user != null)
            {
                if (!_context.Users.Any(u => u.Email == user.Email))
                {
                    user.Password = ComputeSha256Hash(user.Password);
                    _context.Users.Add(user);
                }
            }
            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
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