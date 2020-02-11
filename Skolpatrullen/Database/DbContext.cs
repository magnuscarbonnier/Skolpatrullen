using Microsoft.EntityFrameworkCore;
using System;

namespace Database.Models
{
    public class DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CourseParticipant> UserCourses { get; set; }
        public DbSet<UserSchool> UserSchools { get; set; }

    }
}
