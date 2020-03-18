using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class CourseFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FileId { get; set; }
        public int CourseId { get; set; }
        public File File { get; set; }
        public Course Course { get; set; }
    }
}

