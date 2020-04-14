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
        public DbSet<LoginSession> LoginSessions { get; set; }
        public DbSet<PasswordRecovery> PasswordRecoveries { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<CourseFile> CourseFiles { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<CourseBlogPost> CourseBlogPosts { get; set; }
        public DbSet<StartBlogPost> StartBlogPosts { get; set; }
        public DbSet<AssignmentFile> AssignmentFiles { get; set; }
        public DbSet<UserAssignment> UserAssignments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Skolpatrullen;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CourseRoom>().HasKey(cr => new { cr.CourseId, cr.RoomId });

            builder.Entity<User>().Property(u => u.IsSuperUser).HasDefaultValue(false);

            builder.Entity<User>().Property(u => u.ProfilePictureId).HasDefaultValue(null);
            builder.Entity<User>().HasOne(u => u.ProfilePicture).WithMany(pp => pp.Users).OnDelete(DeleteBehavior.SetNull);
        }

        public Context()
        {

        }
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
    }
}
