using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class File
    {
        public int Id { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public byte[] Binary { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }
        [Required]
        public FileTypes Type { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
    public enum FileTypes
    {
        ProfilePicture = 0,
        Assignment = 1,
        UserAssignment = 2,
        CourseFile = 3,
        CoursePicture = 4
    }
}
