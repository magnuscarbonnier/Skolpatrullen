using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CourseParticipant> UserCourses { get; set; }
        public School School { get; set; }
    }
}
