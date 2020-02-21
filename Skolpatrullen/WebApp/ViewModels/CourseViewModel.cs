using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class CourseViewModel
    {
        public int SchoolId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<School> SchoolList { get; set; }

        public Course ToCourse()
        {
            return new Course
            {
                Name = this.Name,
                SchoolId = this.SchoolId,
                StartDate = this.StartDate,
                EndDate = this.EndDate
            };
        }

    }
}
