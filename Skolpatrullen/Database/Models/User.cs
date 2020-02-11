using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Cell { get; set; }
        public string Home { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public bool IsAdmin { get; set; }
        public string Country { get; set; }
        public ICollection<CourseParticipant> UserCourses { get; set; }
        public ICollection<UserSchool> UserSchools { get; set; }
    }
}
