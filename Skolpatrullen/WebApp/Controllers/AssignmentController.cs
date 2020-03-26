using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

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
        [HttpGet]
        [Route("[controller]/CourseAssignments/{courseid}")]
        public async Task<IActionResult> CourseAssignments(int courseId)
        {
            string message = await GetUser();
            if (User != null)
            {
                IEnumerable<Assignment> assignments = new List<Assignment>();
                var response = await APIGetAssignmentByCourseId(courseId);
                if (response.Data != null)
                {
                    assignments = response.Data.Select(a => new Assignment()
                    {
                        Id = a.Id,
                        CourseId = courseId,
                        Name = a.Name,
                        Deadline = a.Deadline,
                        Description = a.Description
                    });
                    return View(assignments);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("[controller]/{id}")]
        public async Task<IActionResult> GetAssignmentById(int id)
        {
            string message = await GetUser();
            var model = new AssignmentViewModel();

            var assignment = await APIGetAssignmentById(id);
            if (assignment.Data != null)
            {
                model.CourseId = assignment.Data.CourseId;
                model.Deadline = assignment.Data.Deadline;
                model.Description = assignment.Data.Description;
                model.Name = assignment.Data.Name;

                return View("AssignmentDetails", model);
            }
            else
            {
                return NotFound();
            }
        }
    }
}