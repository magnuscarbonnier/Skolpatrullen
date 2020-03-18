using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    class CourseFile
    {
        public string Name { get; set; }
        File File { get; set; }
        Course Course { get; set; }
    }
}
