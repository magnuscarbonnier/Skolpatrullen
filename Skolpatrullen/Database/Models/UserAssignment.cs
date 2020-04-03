using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class UserAssignment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AssignmentId { get; set; }
        public string Grade { get; set; }
        public string Description { get; set; }
        public DateTime ReturnDate { get; set; }

        public User User { get; set; }
        public Assignment Assignment { get; set; }
    }
}
