namespace DevKid.src.Domain.Entities
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid ChapterId { get; set; }
        public Chapter Chapter { get; set; } = null!;
        public List<Material> Materials { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<Quiz> Quizzes { get; set; } = new();
    }
}
