using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{ 
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public ICollection<UserSchool> UserSchools { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
