namespace DevKid.src.Domain.Entities
{
    public class Chapter
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public List<Lesson> Lessons { get; set; } = new();
    }
}
