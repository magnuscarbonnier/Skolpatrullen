using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    class CourseFile
    {
        public string Name { get; set; }
        public File File { get; set; }
        public Course Course { get; set; }
    }
}

