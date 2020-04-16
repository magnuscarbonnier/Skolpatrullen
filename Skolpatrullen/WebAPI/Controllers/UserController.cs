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
    public class UserController : APIController
    {
        public UserController(Context context, ILogger<UserController> logger) : base(context, logger)
        {
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
                response.SuccessMessage = $"Användare med mejladdress {user.Email} är nu registrerad";
            }
            else
            {
                response.FailureMessage = $"Det finns redan en användare med mejladressen {user.Email}";
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
                response.FailureMessage = $"Det finns ingen användare med mejladressen {login.Email}";
                response.Success = false;
            }
            else if (user.Password == ComputeSha256Hash(login.Password))
            {
                response.Data = AddOrUpdateLoginSession(user);
                response.Success = true;
                response.SuccessMessage = $"Användare med mejladdress {user.Email} är nu inloggad";
            }
            else
            {
                response.FailureMessage = $"Fel lösenord";
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
                response.FailureMessage = $"Hittade ingen LoginSession med token: {token}";
                response.Success = false;
            }
            else if (session.Expires < DateTime.Now.ToUniversalTime())
            {
                response.FailureMessage = $"LoginSession har gått ut";
                response.Success = false;
            }
            else
            {
                UpdateLoginSession(session);
                File profilePic = _context.Files.SingleOrDefault(f => f.Id == session.User.ProfilePictureId);
                if (profilePic != null)
                {
                    profilePic.Users = null;
                }
                session.User.ProfilePicture = profilePic;
                response.Data = session;
                response.Success = true;
                response.SuccessMessage = "LoginSession är fortfaraned giltig";
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/Update")]
        public APIResponse<User> Update(User user)
        {
            APIResponse<User> response = new APIResponse<User>();
            var result = _context.Users.SingleOrDefault(u => u.Id == user.Id);
            if (result.Email != user.Email)
            {
                if (!_context.Users.Any(u => u.Email == user.Email))
                {
                    result.FirstName = user.FirstName;
                    result.LastNames = user.LastNames;
                    result.Phone = user.Phone;
                    result.SocialSecurityNr = user.SocialSecurityNr;
                    result.Email = user.Email;
                    result.Address = user.Address;
                    result.City = user.City;
                    result.PostalCode = user.PostalCode;
                    result.IsSuperUser = user.IsSuperUser;
                    _context.SaveChanges();
                    response.Data = user;
                    response.Success = true;
                    response.SuccessMessage = $"Updaterade användare med mejladdress {result.Email}";
                }
                else
                {
                    response.FailureMessage = $"Det finns redan en användare med mejladressen {user.Email}";
                    response.Success = false;
                }
            }
            else
            {
                result.FirstName = user.FirstName;
                result.LastNames = user.LastNames;
                result.Phone = user.Phone;
                result.SocialSecurityNr = user.SocialSecurityNr;
                result.Address = user.Address;
                result.City = user.City;
                result.PostalCode = user.PostalCode;
                result.IsSuperUser = user.IsSuperUser;
                _context.SaveChanges();
                response.Data = user;
                response.Success = true;
                response.SuccessMessage = $"Updaterade användare med mejladdress {result.Email}";
            }

            return response;
        }
        [HttpPost]
        [Route("[controller]/UpdateName")]
        public APIResponse<User> UpdateName(User user)
        {
            APIResponse<User> response = new APIResponse<User>();

            var result = _context.Users.SingleOrDefault(u => u.Id == user.Id);

            result.FirstName = user.FirstName;
            result.LastNames = user.LastNames;
            _context.SaveChanges();
            response.Data = user;
            response.Success = true;
            response.SuccessMessage = $"Updaterade namn på användare med mejladdress {result.Email}";

            return response;
        }
        [HttpPost]
        [Route("[controller]/Logout")]
        public APIResponse Logout(User user)
        {
            APIResponse response = new APIResponse();
            LoginSession session = _context.LoginSessions.SingleOrDefault(ls => ls.UserId == user.Id);
            if (session == null)
            {
                // finns ingen session är användaren redan utloggad
                response.Success = true;
                response.SuccessMessage = "Du är redan utloggad";
            }
            else
            {
                _context.LoginSessions.Remove(session);
                _context.SaveChanges();
                response.Success = true;
                response.SuccessMessage = "Du är nu utloggad";
            }
            return response;
        }
        [HttpPost]
        [Route("[controller]/ChangePassword")]
        public APIResponse ChangePassword(ChangePasswordBody body)
        {
            APIResponse response = new APIResponse();

            User user = _context.Users.SingleOrDefault(u => u.Id == body.UserId);

            if (user != null)
            {
                if (user.Password == ComputeSha256Hash(body.CurrentPassword))
                {
                    user.Password = ComputeSha256Hash(body.NewPassword);
                    _context.SaveChanges();

                    response.Success = true;
                    response.SuccessMessage = $"Ändrade lösenord för användare med mejladdress {user.Email}";
                }
                else
                {
                    response.Success = false;
                    response.FailureMessage = $"Lösenordet du skrev in stämmer inte överrens med ditt nuvarande";
                }
            }
            else
            {
                response.FailureMessage = $"Hittar inte användare med Id: " + body.UserId;
                response.Success = false;
            }

            return response;
        }
        [HttpPost]
        [Route("[controller]/ForceChangePassword")]
        public APIResponse ForceChangePassword(ChangePasswordBody body)
        {
            var response = new APIResponse();
            var user = _context.Users.SingleOrDefault(u => u.Id == body.UserId);
            if (user != null)
            {
                user.Password = ComputeSha256Hash(body.NewPassword);
                _context.SaveChanges();
                response.Success = true;
                response.SuccessMessage = $"Ändrade lösenord för användare med mejladdress {user.Email}";
            }
            else
            {
                response.FailureMessage = $"Hittar inte användare med Id: " + body.UserId;
                response.Success = false;
            }
            return response;
        }

        [HttpGet]
        [Route("[controller]/GetUserById/{Id}")]
        public APIResponse<User> GetUserById(int Id)
        {
            APIResponse<User> response = new APIResponse<User>();
            response.Data = _context.Users.SingleOrDefault(c => c.Id == Id);

            response.Success = true;
            response.SuccessMessage = $"Hämtade användare med id {Id}";
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetStudentsByCourseId/{courseId}")]
        public APIResponse<IEnumerable<User>> GetStudentsByCourseId(int courseId)
        {
            APIResponse<IEnumerable<User>> response = new APIResponse<IEnumerable<User>>();
            response.Data = _context.CourseParticipants
                  .Include(cp => cp.User)
                  .Where(cp => cp.CourseId == courseId && cp.Status==Status.Antagen && cp.Role==Roles.Student).Select(cp => cp.User);

            response.Success = true;
            response.SuccessMessage = $"Hämtade användare med id {courseId}";
            return response;
        }
        [HttpGet]
        [Route("[controller]/GetAllUsers")]
        public APIResponse<IEnumerable<User>> GetAllUsers()
        {
            APIResponse<IEnumerable<User>> response = new APIResponse<IEnumerable<User>>();
            response.Data = _context.Users.ToList();
            response.Success = true;
            response.SuccessMessage = "Hämtade alla användare";
            return response;
        }
        [HttpPost]
        [Route("[controller]/ChangeProfilePicture")]
        public APIResponse ChangeProfilePicture(ChangeProfilePictureBody body)
        {
            APIResponse response = new APIResponse();

            User user = _context.Users.SingleOrDefault(u => u.Id == body.UserId);


            if (user != null)
            {

                File file = new File();

                file.Binary = body.ProfilePicture;
                file.UploadDate = body.UploadDate;
                file.ContentType = body.ContentType;
                file.Name = body.Name;

                _context.Files.Add(file);

                if (user.ProfilePictureId != null)
                {
                    var currentPic = _context.Files.SingleOrDefault(c => c.Id == user.ProfilePictureId);

                    if (currentPic != null)
                    {
                        _context.Remove(currentPic);
                    }
                }
                _context.SaveChanges();
                user.ProfilePictureId = file.Id;
                _context.SaveChanges();

                response.Success = true;

            }
            else
            {
                response.FailureMessage = "Hittar inte användare med Id: " + user.Id;
                response.Success = false;
            }

            return response;
        }
        [HttpGet]
        [Route("[controller]/GetCourseRole/{userId}/{courseId}")]
        public APIResponse<Roles> GetCourseRole(int userId, int courseId)
        {
            var response = new APIResponse<Roles>();
            var participant = _context.CourseParticipants.SingleOrDefault(cp => cp.UserId == userId && cp.CourseId == courseId);
            if (participant != null)
            {
                response.Data = participant.Role;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.FailureMessage = $"Hittade inte någon kursdeltagare med id {userId} för kurs med id {courseId}";
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/IsSchoolAdmin/{userId}/{schoolId}")]
        public APIResponse<bool> IsSchoolAdmin(int userId, int schoolId)
        {
            var response = new APIResponse<bool>();
            var userSchool = _context.UserSchools.SingleOrDefault(us => us.UserId == userId && us.SchoolId == schoolId);
            if (userSchool != null)
            {
                response.Data = userSchool.IsAdmin;
                response.Success = true;
            }
            else
            {
                response.Data = false;
                response.Success = false;
                response.FailureMessage = $"Hittade inte någon skoldeltagare med id {userId} för skola med id {schoolId}";
            }
            return response;
        }
        [HttpGet]
        [Route("[controller]/Search/{Search}")]
        public APIResponse<IEnumerable<User>> GetUsersBySearchString(string Search)
        {
            var search = Search.ToLower().Trim();
            APIResponse<IEnumerable<User>> response = new APIResponse<IEnumerable<User>>();
            response.Data = _context.Users.Where(u => (u.FirstName.ToLower() + " " + u.LastNames.ToLower()).Contains(search) || u.SocialSecurityNr.Contains(search));
            response.Success = true;
            response.SuccessMessage = $"Hämtade alla användare med söksträng {Search}";
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
    }
}