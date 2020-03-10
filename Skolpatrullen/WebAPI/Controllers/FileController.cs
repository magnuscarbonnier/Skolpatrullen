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
    }
}
