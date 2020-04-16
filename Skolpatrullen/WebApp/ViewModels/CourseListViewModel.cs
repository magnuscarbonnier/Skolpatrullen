using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class CourseListViewModel
    {
        public IEnumerable<School> SchoolList { get; set; }
        public IEnumerable<Course> CourseList { get; set; }
        public string Search { get; set; }
        public School School { get; set; }
    }
   
}
