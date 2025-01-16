namespace DevKid.src.Domain.Entities
{
    public class Student : User
    {
        public DateTime DayOfBirth { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public List<StudentCourse> StudentCourses { get; set; } = null!;
        public List<Feedback> Feedbacks { get; set; } = new();
    }
}
