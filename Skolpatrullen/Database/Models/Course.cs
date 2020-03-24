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

        public ICollection<CourseParticipant> CourseParticipants { get; set; }
        public ICollection<CourseRoom> CourseRooms { get; set; }
        public ICollection<Lesson> CourseLessons { get; set; }
        public IEnumerable<CourseBlogPost> CourseBlogPosts { get; set; }
        public School School { get; set; }
    }
}
