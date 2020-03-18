using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class AdminCourseParticipantViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public Roles Role { get; set; }
        public Grade Grade { get; set; }
        public Status Status { get; set; }
        public Course Course { get; set; }
        public User User { get; set; }
        public IEnumerable<CourseParticipant> CourseParticipantList { get; set; }
        public IEnumerable<School> SchoolList { get; set; }
        public CourseParticipant ToCourseParticipant()
        {
            return new CourseParticipant
            {
                Id = this.Id,
                UserId = this.UserId,
                CourseId = this.CourseId,
                Role = this.Role,
                Grade = this.Grade,
                Status = this.Status
            };
        }
    }
}
