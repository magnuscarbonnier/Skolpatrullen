using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Course
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<CourseParticipant> UserCourses { get; set; }
        public School School { get; set; }
        public Room Room { get; set; }
    }
}
