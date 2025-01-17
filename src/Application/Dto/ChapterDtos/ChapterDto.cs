using DevKid.src.Domain.Entities;

namespace DevKid.src.Application.Dto.ChapterDtos
{
    public class ChapterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid CourseId { get; set; }
        public List<Lesson> Lessons { get; set; } = new();
    }
}
