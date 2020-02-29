using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class AdminCourseParticipantViewModel
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public Roles Role { get; set; }
        public string Grade { get; set; }
        public Status Status { get; set; }
        public IEnumerable<Course> CourseList { get; set; }
        public IEnumerable<CourseParticipant> CourseParticipantList { get; set; }
    }
}
