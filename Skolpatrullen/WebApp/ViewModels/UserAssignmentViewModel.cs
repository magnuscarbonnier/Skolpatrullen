using Database.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class UserAssignmentViewModel
    {
        public int UserId { get; set; }
        public int AssignmentId { get; set; }
        public string Grade { get; set; }
        public string Description { get; set; }
        public DateTime ReturnDate { get; set; }
        public Assignment Assignment{ get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
