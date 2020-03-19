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

            if (response.Data != null)
            {
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
        [HttpGet]
        [Route("[controller]/GetAllFilesByCourse/{id}")]
        public APIResponse<IEnumerable<FileBody>> GetAllFilesByCourse(int id)
        {
            APIResponse<IEnumerable<FileBody>> response = new APIResponse<IEnumerable<FileBody>>();
            var files = _context.CourseFiles
                .Include(file => file.File)
                .Where(coursefile => coursefile.CourseId == id)
                .Select(comb => new FileBody
                {
                    CourseId = comb.CourseId,
                    File = comb.File.Binary,
                    Name = comb.Name,
                    UploadDate = comb.File.UploadDate
                });
            if (files != null)
            {
                response.Data = files;
                response.Success = true;
                response.SuccessMessage = "Hämtade alla filer";
            } else
            {
                response.Success = false;
                response.FailureMessage = "Hämtade inga filer";
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetCourseFileById/{id}")]
        public APIResponse<File> GetCourseFileById(int id)
        {
            APIResponse<File> response = new APIResponse<File>();
            var file = _context.Files.SingleOrDefault(f => f.Id == id);
            if(file != null)
            {
                response.Data = file;
                response.Success = true;
                response.SuccessMessage = $"Hämtade filen med id:{id}";
            }
            else
            {
                response.Success = false;
                response.FailureMessage = $"Fanns ingen fil med id:{id}";
            }
            return response;
        }
    }
}
