using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CourseParticipant> UserCourses { get; set; }
    }
}
