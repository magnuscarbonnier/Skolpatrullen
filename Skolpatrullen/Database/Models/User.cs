using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastNames { get; set; }
        public string Phone { get; set; }
        public string SocialSecurityNr { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<CourseParticipant> UserCourses { get; set; }
        public ICollection<UserSchool> UserSchools { get; set; }
    }
}
