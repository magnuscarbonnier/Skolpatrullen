using Database.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class AssignmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<IFormFile> File { get; set; }
        public IEnumerable<File> AssignmentFiles { get; set; }
        public bool TurnedIn { get; set; }
        public bool IsTeacher { get; set; }
    }
}
