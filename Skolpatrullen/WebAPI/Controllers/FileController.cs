using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.ViewModels;

namespace WebAPI.Controllers
{
    public class FileController : APIController
    {
        public FileController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpGet]
        [Route("[controller]/GetFileById/{Id}")]
        public APIResponse<File> GetFileById(int Id)
        {
            APIResponse<File> response = new APIResponse<File>();
            response.Data = _context.Files.SingleOrDefault(c => c.Id == Id);

            response.Success = true;
            response.SuccessMessage = $"Hämtade fil med id {Id}";
            return response;
        }

        [HttpGet]
        [Route("[controller]/DeleteFileById/{Id}")]
        public APIResponse<File> DeleteFileById(int Id)
        {
            APIResponse<File> response = new APIResponse<File>();
            response.Data = _context.Files.SingleOrDefault(c => c.Id == Id);
            APIResponse<User> userResponse = new APIResponse<User>();
            userResponse.Data = _context.Users.SingleOrDefault(c => c.ProfilePictureId == Id);

            if (response.Data != null && userResponse.Data != null)
            {
                userResponse.Data.ProfilePictureId = null;
                _context.SaveChanges();

                _context.Remove(response.Data);
                _context.SaveChanges();
                response.SuccessMessage = $"Tog bort fil med id {Id}";
            }
            else
            {
                response.SuccessMessage = $"Det fanns ingen fil med id {Id}";
            }
            response.Success = true;


            return response;

        }

    }
}
