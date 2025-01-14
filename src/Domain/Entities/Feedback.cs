namespace DevKid.src.Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
        public string? Description { get; set; }
        public float Rating { get; set; }
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
