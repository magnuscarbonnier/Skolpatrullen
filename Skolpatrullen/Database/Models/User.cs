using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastNames { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string SocialSecurityNr { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Range(8,16)]
        public string Password { get; set; }
        public ICollection<CourseParticipant> CourseParticipants { get; set; }
        public ICollection<UserSchool> UserSchools { get; set; }
    }
}
