using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class CourseParticipant
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public Roles Role { get; set; }
        public string Grade { get; set; }
        public Status Status { get; set; }
       
        public User User { get; set; }
        public Course Course { get; set; }
    }
}
