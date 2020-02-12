using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class UserSchool
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
