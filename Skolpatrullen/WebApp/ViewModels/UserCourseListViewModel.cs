using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApp.ViewModels
{
    public class UserCourseListViewModel
    {
        public IEnumerable<CourseParticipant> CourseParticipantList { get; set; }
        public IEnumerable<School> SchoolList { get; set; }
    }
}