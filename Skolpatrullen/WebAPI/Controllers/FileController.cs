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
    [ApiController]
    public class FileController : APIController
    {
        public FileController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
        }

        [HttpGet]
        [Route("[controller]/GetFileById/{id}")]
        public APIResponse<File> GetFileById(int id)
        {
            APIResponse<File> response = new APIResponse<File>();
            var file = _context.Files.SingleOrDefault(c => c.Id == id);
            if (file != null)
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
        public APIResponse<IEnumerable<File>> GetAllFilesByCourse(int id)
        {
            APIResponse<IEnumerable<File>> response = new APIResponse<IEnumerable<File>>();
            var files = _context.CourseFiles.Include(cf => cf.File).Where(cf => cf.CourseId == id).Select(cf => cf.File);
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
        [HttpPost]
        [Route("[controller]/UploadCourseFile")]
        public APIResponse UploadCourseFile(CourseFileBody body)
        {
            APIResponse response = new APIResponse();

            User user = _context.Users.SingleOrDefault(u => u.Id == body.UserId);
            if (user != null && body.File.Length > 0)
            {
                File file = new File();
                CourseFile coursefile = new CourseFile();

                file.Binary = body.File;
                file.UploadDate = body.UploadDate;
                file.ContentType = body.ContentType;
                file.Type = FileTypes.CourseFile;
                file.Name = body.Name;

                _context.Files.Add(file);
                _context.SaveChanges();

                coursefile.CourseId = body.CourseId;
                coursefile.FileId = file.Id;

                _context.CourseFiles.Add(coursefile);
                _context.SaveChanges();

                response.Success = true;
            }
            else
            {
                response.FailureMessage = "Filen laddades inte upp";
                response.Success = false;
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/UploadAssignmentFile")]
        public APIResponse UploadAssignmentFile(AssignmentFileBody body)
        {
            APIResponse response = new APIResponse();

            //checks if user exists
            User user = _context.Users.SingleOrDefault(u => u.Id == body.UserId);


            if (user != null && body.File.Length > 0)
            {
                File file = new File();
                AssignmentFile assignmentfile = new AssignmentFile();

                file.Binary = body.File;
                file.UploadDate = body.UploadDate;
                file.ContentType = body.ContentType;
                file.Type = body.Type;
                file.Name = body.Name;

                _context.Files.Add(file);
                _context.SaveChanges();

                assignmentfile.AssignmentId = body.AssignmentId;
                assignmentfile.FileId = file.Id;
                if (body.Type == FileTypes.UserAssignment)
                {
                    assignmentfile.Type = AssignmentFileType.StudentFile;
                } else if (body.Type == FileTypes.Assignment)
                {
                    assignmentfile.Type = AssignmentFileType.AssignmentFile;
                }
                assignmentfile.UserId = body.UserId;

                _context.AssignmentFiles.Add(assignmentfile);
                _context.SaveChanges();

                response.SuccessMessage = "Laddade upp inlämningsfil";
                response.Success = true;
            }
            else
            {
                response.FailureMessage = "Filen laddades inte upp";
                response.Success = false;
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetFilesByAssignment/{AssignmentId}")]
        public APIResponse<IEnumerable<File>> GetFilesByAssignment(int AssignmentId)
        {
            APIResponse<IEnumerable<File>> response = new APIResponse<IEnumerable<File>>();
            var files = _context.AssignmentFiles.Include(af => af.File).Where(af => af.AssignmentId == AssignmentId).Select(af => af.File);
            if (files != null)
            {
                response.Data = files;
                response.Success = true;
                response.SuccessMessage = "Hämtade alla filer";
            }
            else
            {
                response.Success = false;
                response.FailureMessage = "Hämtade inga filer";
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetUserAssignmentFilesByUserId/{UserId}")]
        public APIResponse<IEnumerable<AssignmentFile>> GetUserAssignmentFilesByUserId(int UserId)
        {
            APIResponse<IEnumerable<AssignmentFile>> response = new APIResponse<IEnumerable<AssignmentFile>>();
            var files = _context.AssignmentFiles.Include(af => af.File).Where(af => af.UserId == UserId);
            if (files != null)
            {
                response.Data = files;
                response.Success = true;
                response.SuccessMessage = $"Hämtade alla filer som tillhör Id: {UserId}";
            }
            else
            {
                response.Success = false;
                response.FailureMessage = "Hämtade inga filer";
            }
            return response;
        }
    }
}
