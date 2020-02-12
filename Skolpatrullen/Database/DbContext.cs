using Microsoft.EntityFrameworkCore;
using System;

namespace Database.Models
{
    public class Context : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseParticipant> CourseParticipants { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSchool> UserSchools { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Skolpatrullen;Trusted_Connection=True;");
        }

    }
}
