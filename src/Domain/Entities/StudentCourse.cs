namespace DevKid.src.Domain.Entities
{
    public class StudentCourse
    {
        public enum StudentCourseStatus
        {
            InProgress,
            Completed
        }
        public StudentCourseStatus Status { get; set; } = StudentCourseStatus.InProgress;
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
