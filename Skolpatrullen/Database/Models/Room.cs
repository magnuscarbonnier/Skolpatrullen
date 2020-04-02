using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public int SchoolId { get; set; }
        public string Name { get; set; }

        public School School { get; set; }
        public IEnumerable<CourseRoom> CourseRooms { get; set; }
    }
}
