namespace DevKid.src.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public float Price { get; set; }
        public List<Chapter> Chapters { get; set; } = new();
    }
}
