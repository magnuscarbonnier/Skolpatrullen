using Database.Models;
using Lib;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public abstract class AppController : Controller
    {
        private readonly HttpClient HttpClient;
        public new User User = null;
        public AppController()
        {
            HttpClient = new HttpClient();
        }

        public async Task<string> GetUser()
        {
            ViewBag.User = null;
            ViewBag.CountCP = 0;
            KeyValuePair<string, string>? cookie = Request.Cookies.SingleOrDefault(c => c.Key == "LoginToken");
            if (cookie.Value.Value == null)
            {
                return "Inte inloggad";
            }
            var response = await APIGetLoginSession(cookie.Value.Value);
            
            if (response.Success)
            {
                User = response.Data.User;
                ViewBag.User = User;
                if (User.IsSuperUser == true)
                {
                    var countCP = await APIGetCourseParticipantsNotYetAccepted();
                    ViewBag.CountCP = countCP.Data;
                }
                return response.SuccessMessage;
            }
            else
            {
                return response.FailureMessage;
            }
            return "Något gick fel ¯\\_(ツ)_/¯";
        }
        public void SetFailureMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                TempData["ErrorMessage"] = "Något gick fel";
            }
            else
            {
                TempData["ErrorMessage"] = message;
            }
        }
        public void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }
        public void SetResponseMessage(APIResponse response)
        {
            if (response.Success)
            {
                SetSuccessMessage(response.SuccessMessage);
            }
            else
            {
                SetFailureMessage(response.FailureMessage);
            }
        }
        public IActionResult SetResponseMessage(APIResponse response, IActionResult successView, IActionResult failureView)
        {
            if (response.Success)
            {
                SetSuccessMessage(response.SuccessMessage);
                return successView;
            }
            else
            {
                SetFailureMessage(response.FailureMessage);
                return failureView;
            }
        }
        public async Task<HttpResponseMessage> APIPost<T>(string route, T body)
        {
            var json = JsonConvert.SerializeObject(body);
            using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await HttpClient.PostAsync("https://localhost:44367" + route, stringContent);
                return response;
            }
        }
        public async Task<HttpResponseMessage> APIGet(string route)
        {
            HttpResponseMessage response = await HttpClient.GetAsync("https://localhost:44367" + route);
            return response;
        }
        public async Task<APIResponse<LoginSession>> APILogin(LoginViewModel loginVM)
        {
            HttpResponseMessage response = await APIPost("/User/Login", loginVM);
            return (APIResponse<LoginSession>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<LoginSession>));
        }
        public async Task<APIResponse<LoginSession>> APIGetLoginSession(string token)
        {
            HttpResponseMessage response = await APIPost("/User/GetLoginSession", new TokenBody { token = token });
            return (APIResponse<LoginSession>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<LoginSession>));
        }
        public async Task<APIResponse<LoginSession>> APIRegister(UserViewModel UserVM)
        {
            HttpResponseMessage response = await APIPost("/User/Register", UserVM);
            return (APIResponse<LoginSession>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<LoginSession>));
        }
        public async Task<APIResponse> APILogout(User user)
        {
            HttpResponseMessage response = await APIPost("/User/Logout", user);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<IEnumerable<CourseParticipant>>> APIGetAllCourseParticipants()
        {
            HttpResponseMessage response = await APIGet("/CourseParticipant/GetAll");
            return (APIResponse<IEnumerable<CourseParticipant>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<CourseParticipant>>));
        }
        public async Task<APIResponse<IEnumerable<Course>>> APIGetAllCourses()
        {
            HttpResponseMessage response = await APIGet("/Course/GetAll");
            return (APIResponse<IEnumerable<Course>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<Course>>));
        }
        public async Task<APIResponse<IEnumerable<School>>> APIGetAllSchools()
        {
            HttpResponseMessage response = await APIGet("/School/GetAllSchools");
            return (APIResponse<IEnumerable<School>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<School>>));
        }
        public async Task<APIResponse<IEnumerable<UserSchool>>> APIGetAllUserSchools()
        {
            HttpResponseMessage response = await APIGet("/UserSchool/GetAllUserSchools");
            return (APIResponse<IEnumerable<UserSchool>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<UserSchool>>));
        }
        public async Task<APIResponse<IEnumerable<User>>> APIGetAllUsers()
        {
            HttpResponseMessage response = await APIGet("/User/GetAllUsers");
            return (APIResponse<IEnumerable<User>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<User>>));
        }
        public async Task<APIResponse<Room>> APIAddRoom(Room room)
        {
            HttpResponseMessage response = await APIPost("/Room/Add", room);
            return (APIResponse<Room>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<Room>));
        }
        public async Task<APIResponse<UserSchool>> APIAddOrUpdateUserSchool(UserSchool userSchool)
        {
            HttpResponseMessage response = await APIPost("/UserSchool/AddOrUpdate", userSchool);
            return (APIResponse<UserSchool>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<UserSchool>));
        }
        public async Task<APIResponse<CourseParticipant>> APIAddOrUpdateCourseParticipant(CourseParticipant courseParticipant)
        {
            HttpResponseMessage response = await APIPost("/CourseParticipant/AddOrUpdate", courseParticipant);
            return (APIResponse<CourseParticipant>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<CourseParticipant>));
        }
        public async Task<APIResponse<IEnumerable<CourseParticipant>>> APIGetCourseParticipantsByUserId(int Id)
        {
            HttpResponseMessage response = await APIGet("/CourseParticipant/GetCourseParticipantsByUserId/" + Id);
            return (APIResponse<IEnumerable<CourseParticipant>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<CourseParticipant>>));
        }
        public async Task<APIResponse<CourseParticipant>> APIGetCourseParticipantById(int Id)
        {
            HttpResponseMessage response = await APIGet("/CourseParticipant/GetCourseParticipantById/" + Id);
            return (APIResponse<CourseParticipant>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<CourseParticipant>));
        }
        public async Task<APIResponse<Course>> APIAddCourse(Course course)
        {
            HttpResponseMessage response = await APIPost("/Course/Add", course);
            return (APIResponse<Course>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<Course>));
        }
        public async Task<APIResponse> APIRemoveCourse(int id)
        {
            HttpResponseMessage response = await APIGet("/Course/RemoveCourse/" + id);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<User>> APIUpdateUser(User user)
        {
            HttpResponseMessage response = await APIPost("/User/Update", user);
            return (APIResponse<User>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<User>));
        }
        public async Task<APIResponse> APIChangePassword(ChangePasswordBody body)
        {
            HttpResponseMessage response = await APIPost("/User/ChangePassword", body);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse> APIForceChangePassword(ChangePasswordBody body)
        {
            HttpResponseMessage response = await APIPost("/User/ForceChangePassword", body);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<School>> APIAddSchool(School school)
        {
            HttpResponseMessage response = await APIPost("/School/AddSchool", school);
            return (APIResponse<School>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<School>));
        }
        public async Task<APIResponse> APIRemoveSchool(int id)
        {
            HttpResponseMessage response = await APIGet("/School/RemoveSchool/" + id);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<User>> APIGetUserById(int Id)
        {
            HttpResponseMessage response = await APIGet("/User/GetUserById/" + Id.ToString());
            return (APIResponse<User>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<User>));
        }
        public async Task<APIResponse> APIChangeProfilePicture(ChangeProfilePictureBody body)
        {
            HttpResponseMessage response = await APIPost("/User/ChangeProfilePicture", body);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<Course>> APIGetCourseById(int Id)
        {
            HttpResponseMessage response = await APIGet("/Course/GetCourseById/" + Id.ToString());
            return (APIResponse<Course>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<Course>));
        }
        public async Task<APIResponse<PasswordRecovery>> APIRecoverPasswordWithEmail(string email)
        {
            HttpResponseMessage response = await APIGet("/PasswordRecovery/ByEmail/" + email);
            return (APIResponse<PasswordRecovery>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<PasswordRecovery>));
        }
        public async Task<APIResponse<PasswordRecovery>> APIGetPasswordRecoveryByToken(string token)
        {
            HttpResponseMessage response = await APIGet("/PasswordRecovery/GetByToken/" + token);
            return (APIResponse<PasswordRecovery>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<PasswordRecovery>));
        }
        public async Task<APIResponse<PasswordRecovery>> APIDeletePasswordRecoveryByToken(string token)
        {
            HttpResponseMessage response = await APIGet("/PasswordRecovery/DeleteByToken/" + token);
            return (APIResponse<PasswordRecovery>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<PasswordRecovery>));
        }
        public async Task<APIResponse<IEnumerable<LessonViewModel>>> APIGetAllLessons()
        {
            HttpResponseMessage response = await (APIGet("/Lesson/"));
            return (APIResponse<IEnumerable<LessonViewModel>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<LessonViewModel>>));
        }
        public async Task<APIResponse<File>> APIGetFileById(int Id)
        {
            HttpResponseMessage response = await APIGet("/File/GetFileById/" + Id.ToString());
            return (APIResponse<File>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<File>));
        }
        public async Task<APIResponse<File>> APIDeleteFileById(int Id)
        {
            HttpResponseMessage response = await APIGet("/File/DeleteFileById/" + Id.ToString());
            return (APIResponse<File>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<File>));
        }
        public async Task<APIResponse<Roles>> APIGetCourseRole(int userId, int courseId)
        {
            HttpResponseMessage response = await APIGet($"/User/GetCourseRole/{userId}/{courseId}");
            return (APIResponse<Roles>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<Roles>));
        }
        public async Task<APIResponse<bool>> APIIsSchoolAdmin(int userId, int schoolId)
        {
            HttpResponseMessage response = await APIGet($"/User/IsSchoolAdmin/{userId}/{schoolId}");
            return (APIResponse<bool>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<bool>));
        }
        public async Task<APIResponse> APIUploadCourseFile(CourseFileBody body)
        {
            HttpResponseMessage response = await APIPost("/File/UploadCourseFile/", body);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<IEnumerable<File>>> APIGetAllCourseFiles(int courseId)
        {
            HttpResponseMessage response = await APIGet($"/File/GetAllFilesByCourse/{courseId}");
            return (APIResponse<IEnumerable<File>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<File>>));
        }
        public async Task<APIResponse<Assignment>> APIAddAssignment(Assignment assignment)
        {
            HttpResponseMessage response = await APIPost("/Assignment/Add/", assignment);
            return (APIResponse<Assignment>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<Assignment>));
        }
        public async Task<APIResponse<IEnumerable<Assignment>>> APIGetAssignmentByCourseId(int id)
        {
            HttpResponseMessage response = await APIGet("/Assignment/GetAssignmentByCourse/" + id);
            return (APIResponse<IEnumerable<Assignment>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<Assignment>>));
        }
        public async Task<APIResponse<IEnumerable<LessonViewModel>>> APIGetUserLessons(int userid)
        {
            HttpResponseMessage response = await (APIGet($"/Lesson/UserLessons/{userid}"));
            return (APIResponse<IEnumerable<LessonViewModel>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<LessonViewModel>>));
        }
        public async Task<APIResponse<IEnumerable<CourseBlogPost>>> APIGetBlogPostsByCourseId(int id)
        {
            HttpResponseMessage response = await (APIGet("/CourseBlog/GetBlogPostsByCourseId/" + id));
            return (APIResponse<IEnumerable<CourseBlogPost>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<CourseBlogPost>>));
        }
        public async Task<APIResponse> APIAddBlogPost(CourseBlogPost blogPost)
        {
            HttpResponseMessage response = await APIPost("/CourseBlog/Add/", blogPost);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<Assignment>> APIGetAssignmentById(int Id)
        {
            HttpResponseMessage response = await APIGet("/Assignment/GetAssignmentById/" + Id.ToString());
            return (APIResponse<Assignment>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<Assignment>));
        }
        public async Task<APIResponse> APIUploadAssignmentFile(AssignmentFileBody body)
        {
            HttpResponseMessage response = await APIPost("/File/UploadAssignmentFile/", body);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse> APIRemoveBlogPost(int id)
        {
            HttpResponseMessage response = await APIGet($"/CourseBlog/Remove/{id}");
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse> APIRemoveCourseParticipant(int id)
        {
            HttpResponseMessage response = await APIGet($"/CourseParticipant/Remove/{id}");
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<IEnumerable<Course>>> APIGetCoursesByUserId(int id)
        {
            HttpResponseMessage response = await APIGet($"/Course/GetCoursesByUserId/{id}");
            return (APIResponse<IEnumerable<Course>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<Course>>));
        }
        public async Task<APIResponse<IEnumerable<School>>> APIGetSchoolsByUserId(int id)
        {
            HttpResponseMessage response = await APIGet($"/School/GetSchoolsByUserId/{id}");
            return (APIResponse<IEnumerable<School>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<School>>));
        }
        public async Task<APIResponse<IEnumerable<File>>> APIGetFilesByAssignment(int id)
        {
            HttpResponseMessage response = await APIGet($"/File/GetFilesByAssignment/{id}");
            return (APIResponse<IEnumerable<File>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<File>>));
        }
        public async Task<APIResponse<bool>> APIUserAssignmentReturnedStatus(int assignmentId, int userId)
        {
            HttpResponseMessage response = await APIGet($"/UserAssignment/IsReturned/{assignmentId}/{userId}");
            return (APIResponse<bool>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<bool>));
        }
        public async Task<APIResponse<IEnumerable<UserAssignment>>> APIGetAllUserAssignmentByUser(int id)
        {
            HttpResponseMessage response = await APIGet($"/UserAssignment/GetAllByUser/{id}");
            return (APIResponse<IEnumerable<UserAssignment>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<UserAssignment>>));
        }
        public async Task<APIResponse<IEnumerable<UserAssignment>>> APIGetAllUserAssignment()
        {
            HttpResponseMessage response = await APIGet($"/UserAssignment/GetAll/");
            return (APIResponse<IEnumerable<UserAssignment>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<UserAssignment>>));
        }
        public async Task<APIResponse> APIAddOrUpdateUserAssignment(UserAssignment userAssignment)
        {
            HttpResponseMessage response = await APIPost("/UserAssignment/AddOrUpdate/", userAssignment);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse> APIRemoveUserAssignment(int id)
        {
            HttpResponseMessage response = await APIGet($"/UserAssignment/Remove/{id}");
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<IEnumerable<StartBlogPost>>> APIGetAllStartBlogPosts()
        {
            HttpResponseMessage response = await APIGet($"/StartBlog/GetAll");
            return (APIResponse<IEnumerable<StartBlogPost>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<StartBlogPost>>));
        }
        public async Task<APIResponse> APIRemoveStartBlogPost(int id)
        {
            HttpResponseMessage response = await APIGet($"/StartBlog/Remove/{id}");
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse> APIAddStartBlogPost(StartBlogPost blogPost)
        {
            HttpResponseMessage response = await APIPost("/StartBlog/Add/", blogPost);
            return (APIResponse)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse));
        }
        public async Task<APIResponse<IEnumerable<User>>> APIGetUsersBySearchString(String Search)
        {
            HttpResponseMessage response = await APIGet($"/User/Search/{Search}");
            return (APIResponse<IEnumerable<User>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<User>>));
        }
        public async Task<APIResponse<IEnumerable<Course>>> APIGetCoursesBySchoolId(int Id)
        {
            HttpResponseMessage response = await APIGet($"/Course/GetCoursesBySchoolId/{Id}");
            return (APIResponse<IEnumerable<Course>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<Course>>));
        }
        public async Task<APIResponse<IEnumerable<UserAssignment>>> APIGetAllUserAssignmentsByAssignmentId(int id)
        {
            HttpResponseMessage response = await APIGet($"/UserAssignment/GetAllByAssignmentId/{id}");
            return (APIResponse<IEnumerable<UserAssignment>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<UserAssignment>>));
        }
        public async Task<APIResponse<IEnumerable<User>>> APIGetStudentsByCourseId(int id)
        {
            HttpResponseMessage response = await APIGet($"/User/GetStudentsByCourseId/{id}");
            return (APIResponse<IEnumerable<User>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<User>>));
        }
        public async Task<APIResponse<UserAssignment>> APIGetUserAssignmentByCourseUserAndAssignment(int CourseId, int UserId, int AssignmentId)
        {
            HttpResponseMessage response = await APIGet($"/UserAssignment/GetByCourseUserAndAssignment/{CourseId}/{UserId}/{AssignmentId}");
            return (APIResponse<UserAssignment>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<UserAssignment>));
        }
        public async Task<APIResponse<UserAssignment>> APIGetUserAssignmentById(int id)
        {
            HttpResponseMessage response = await APIGet($"/UserAssignment/GetUserAssignmentById/{id}");
            return (APIResponse<UserAssignment>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<UserAssignment>));
        }
        public async Task<APIResponse<IEnumerable<AssignmentFile>>> APIGetUserAssignmentFilesByUserId(int UserId)
        {
            HttpResponseMessage response = await APIGet($"/File/GetUserAssignmentFilesByUserId/{UserId}");
            return (APIResponse<IEnumerable<AssignmentFile>>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<IEnumerable<AssignmentFile>>));
        }
        public async Task<APIResponse<int>> APIGetCourseParticipantsNotYetAccepted()
        {
            HttpResponseMessage response = await APIGet($"/CourseParticipant/GetCourseParticipantsNotYetAccepted");
            return (APIResponse<int>)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(APIResponse<int>));
        }
    }
}
