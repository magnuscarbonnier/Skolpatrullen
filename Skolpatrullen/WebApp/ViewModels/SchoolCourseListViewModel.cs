using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SchoolCourseListViewModel
    {
        public IEnumerable<Course> CourseList { get; set; }
        public School School { get; set; }
    }
   
}
