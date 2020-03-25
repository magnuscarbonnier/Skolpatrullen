using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class Course
    {
        
        public int Id { get; set; }
        public int SchoolId { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IEnumerable<CourseParticipant> CourseParticipants { get; set; }
        public IEnumerable<CourseRoom> CourseRooms { get; set; }
        public IEnumerable<Lesson> CourseLessons { get; set; }
        public IEnumerable<CourseBlogPost> CourseBlogPosts { get; set; }
        public School School { get; set; }
    }
}
