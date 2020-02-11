using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class CourseParticipant
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public Role Role { get; set; }
    }
}
