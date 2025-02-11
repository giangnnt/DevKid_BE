using DevKid.src.Application.Constant;

namespace DevKid.src.Domain.Entities
{
    public class Student : User
    {
        public Student()
        {
            RoleId = RoleConst.GetRoleId("STUDENT");
        }
        public DateTime DayOfBirth { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public List<StudentCourse> StudentCourses { get; set; } = null!;
        public List<Feedback> Feedbacks { get; set; } = new();
    }
}
