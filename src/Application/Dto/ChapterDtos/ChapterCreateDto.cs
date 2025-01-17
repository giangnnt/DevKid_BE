namespace DevKid.src.Application.Dto.ChapterDtos
{
    public class ChapterCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid CourseId { get; set; }
    }
}
