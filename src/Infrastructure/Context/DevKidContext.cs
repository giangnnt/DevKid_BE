using DevKid.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DevKid.src.Infrastructure.Context
{
    public class DevKidContext : DbContext
    {
        public DevKidContext()
        {

        }
        public DevKidContext(DbContextOptions<DevKidContext> options) : base(options)
        {

        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.StudentId, sc.CourseId });
            // seed role
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "ADMIN"
                },
                new Role
                {
                    Id = 2,
                    Name = "TEACHER"
                },
                new Role
                {
                    Id = 3,
                    Name = "STUDENT"
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    RoleId = 1,
                    Password = "$2a$11$YiI2CDlblzUkSC2lPOfdk.Y3NHROl1J02rEELG3DOczvG3qvqF9VC",
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    Phone = "0123456789",
                    IsActive = true
                });
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<User>("User")
                .HasValue<Student>("Student");

            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Payment>()
                .Property(p => p.Status)
                .HasConversion<string>();
        }
    }
}
