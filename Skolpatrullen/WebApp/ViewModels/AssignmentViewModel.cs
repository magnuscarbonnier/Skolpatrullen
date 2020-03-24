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

        public IEnumerable<UploadCourseFileViewModel> AssignmentFiles { get; set; }
    }
}
