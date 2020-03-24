using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AssignmentController : AppController
    {
        [HttpPost]
        [Route("[controller]/AddCourseAssignment")]
        public async Task<IActionResult> AddCourseAssignment(Assignment assignment, int courseId)
        {
            string message = await GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            assignment.CourseId = courseId;
            var response = await APIAddAssignment(assignment);

            return RedirectToAction("GetCourseById", "Course", new { Id = assignment.CourseId });
        }
    }
}