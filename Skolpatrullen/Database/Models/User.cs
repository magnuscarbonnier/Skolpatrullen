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
        public string Password { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public bool IsSuperUser { get; set; }
        public int? ProfilePictureId { get; set; }
        public IEnumerable<CourseParticipant> CourseParticipants { get; set; }
        public IEnumerable<UserSchool> UserSchools { get; set; }
        public IEnumerable<StartBlogPost> StartBlogPosts { get; set; }
        public File ProfilePicture { get; set; }
    }
}
