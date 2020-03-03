using Database.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class EditCourseParticipantViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public Roles Role { get; set; }
        public string Grade { get; set; }
        public Status Status { get; set; }
    }
}
