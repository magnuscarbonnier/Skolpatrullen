using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class CourseRoom
    {
        public int CourseId { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public Course Course { get; set; }
    }
}
