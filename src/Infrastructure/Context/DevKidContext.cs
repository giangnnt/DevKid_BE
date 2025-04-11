using DevKid.src.Application.Constant;
using DevKid.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<StudentQuiz> StudentQuizzes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // seed permission
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Slug = PermissionSlug.CHAPTER_ALL, Name = "Chapter All" },
                new Permission { Slug = PermissionSlug.CHAPTER_VIEW, Name = "Chapter View" },
                new Permission { Slug = PermissionSlug.COMMENT_ALL, Name = "Comment All" },
                new Permission { Slug = PermissionSlug.COMMENT_OWN, Name = "Comment Own" },
                new Permission { Slug = PermissionSlug.COMMNET_VIEW, Name = "Comment View" },
                new Permission { Slug = PermissionSlug.COURSE_ALL, Name = "Course All" },
                new Permission { Slug = PermissionSlug.COURSE_VIEW, Name = "Course View" },
                new Permission { Slug = PermissionSlug.FEEDBACK_ALL, Name = "Feedback All" },
                new Permission { Slug = PermissionSlug.FEEDBACK_OWN, Name = "Feedback Own" },
                new Permission { Slug = PermissionSlug.FEEDBACK_VIEW, Name = "Feedback View" },
                new Permission { Slug = PermissionSlug.LESSON_ALL, Name = "Lesson All" },
                new Permission { Slug = PermissionSlug.LESSON_VIEW, Name = "Lesson View" },
                new Permission { Slug = PermissionSlug.MATERIAL_ALL, Name = "Material All" },
                new Permission { Slug = PermissionSlug.MATERIAL_VIEW, Name = "Material View" },
                new Permission { Slug = PermissionSlug.ORDER_ALL, Name = "Order All" },
                new Permission { Slug = PermissionSlug.ORDER_OWN, Name = "Order Own" },
                new Permission { Slug = PermissionSlug.ORDER_VIEW, Name = "Order View" },
                new Permission { Slug = PermissionSlug.PAYMENT_ALL, Name = "Payment All" },
                new Permission { Slug = PermissionSlug.PAYMENT_OWN, Name = "Payment Own" },
                new Permission { Slug = PermissionSlug.PAYMENT_VIEW, Name = "Payment View" },
                new Permission { Slug = PermissionSlug.PERMISSION_ALL, Name = "Permission All" },
                new Permission { Slug = PermissionSlug.PERMISSION_VIEW, Name = "Permission View" },
                new Permission { Slug = PermissionSlug.ROLE_ALL, Name = "Role All" },
                new Permission { Slug = PermissionSlug.ROLE_VIEW, Name = "Role View" },
                new Permission { Slug = PermissionSlug.USER_ALL, Name = "User All" },
                new Permission { Slug = PermissionSlug.USER_OWN, Name = "User Own" },
                new Permission { Slug = PermissionSlug.USER_VIEW_BASIC, Name = "User View Basic" },
                new Permission { Slug = PermissionSlug.USER_VIEW_DETAIL, Name = "User View Detail" },
                new Permission { Slug = PermissionSlug.QUIZ_ALL, Name = "Quiz All" },
                new Permission { Slug = PermissionSlug.STUDENT_QUIZ_ALL, Name = "Student Quiz All" });
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
            // seed permission role
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j
                .HasData(
                    // Admin
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.CHAPTER_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.CHAPTER_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.COMMENT_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.COMMENT_OWN },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.COMMNET_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.COURSE_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.COURSE_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.FEEDBACK_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.FEEDBACK_OWN },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.FEEDBACK_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.LESSON_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.LESSON_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.MATERIAL_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.MATERIAL_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.ORDER_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.ORDER_OWN },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.ORDER_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.PAYMENT_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.PAYMENT_OWN },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.PAYMENT_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.PERMISSION_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.PERMISSION_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.ROLE_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.ROLE_VIEW },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.USER_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.USER_OWN },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.USER_VIEW_BASIC },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.USER_VIEW_DETAIL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.QUIZ_ALL },
                    new { RolesId = 1, PermissionsSlug = PermissionSlug.STUDENT_QUIZ_ALL },
                    // Student
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.COURSE_VIEW },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.LESSON_VIEW },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.MATERIAL_VIEW },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.COMMNET_VIEW },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.COMMENT_OWN },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.FEEDBACK_OWN },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.FEEDBACK_VIEW },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.ORDER_OWN },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.PAYMENT_OWN },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.USER_OWN },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.USER_VIEW_BASIC },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.STUDENT_QUIZ_ALL },
                    new { RolesId = 3, PermissionsSlug = PermissionSlug.CHAPTER_VIEW }
                ));

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

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
                .Property(o => o.CreateAt)
                .HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
            modelBuilder.Entity<Payment>(e =>
            {
                e.Property(e => e.CreateAt)
                    .HasDefaultValueSql("(SYSDATETIMEOFFSET() AT TIME ZONE 'SE Asia Standard Time')");
            });
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Payment>()
                .Property(p => p.Status)
                .HasConversion<string>();
            modelBuilder.Entity<StudentQuiz>()
                .Property(sq => sq.Status)
                .HasConversion<string>();
            // seed course and config
            modelBuilder.Entity<Course>()
                .Property(c => c.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Course>()
                .HasData(
                    new Course
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Name = "C#",
                        Description = "C#",
                        Price = 2000,
                        ImageUrl = "https://static-00.iconduck.com/assets.00/c-sharp-c-icon-456x512-9sej0lrz.png",
                        Status = Course.CourseStatus.Active,
                    },
                    new Course
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Name = "Java",
                        Description = "Java",
                        Price = 3000,
                        ImageUrl = "https://w1.pngwing.com/pngs/800/889/png-transparent-oracle-logo-java-programming-language-javascript-java-development-kit-java-platform-standard-edition-apache-spark-computer-thumbnail.png",
                        Status = Course.CourseStatus.Active,
                    }
                );
            modelBuilder.Entity<Chapter>()
                .HasData(
                    new Chapter
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Name = "Chapter 1",
                        CourseId = Guid.Parse("00000000-0000-0000-0000-000000000001")
                    },
                    new Chapter
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Name = "Chapter 2",
                        CourseId = Guid.Parse("00000000-0000-0000-0000-000000000001")
                    },
                    new Chapter
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                        Name = "Chapter 1",
                        CourseId = Guid.Parse("00000000-0000-0000-0000-000000000002")
                    },
                    new Chapter
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                        Name = "Chapter 2",
                        CourseId = Guid.Parse("00000000-0000-0000-0000-000000000002")
                    }
                );
            modelBuilder.Entity<Lesson>()
                .HasData(
                    new Lesson
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Name = "Lesson 1",
                        ChapterId = Guid.Parse("00000000-0000-0000-0000-000000000001")
                    },
                    new Lesson
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Name = "Lesson 2",
                        ChapterId = Guid.Parse("00000000-0000-0000-0000-000000000001")
                    },
                    new Lesson
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                        Name = "Lesson 1",
                        ChapterId = Guid.Parse("00000000-0000-0000-0000-000000000003")
                    },
                    new Lesson
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                        Name = "Lesson 2",
                        ChapterId = Guid.Parse("00000000-0000-0000-0000-000000000003")
                    }
                );
        }
    }
}
