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
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ICollection<UserSchool> UserSchools { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
