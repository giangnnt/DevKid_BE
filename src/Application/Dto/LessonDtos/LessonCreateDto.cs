namespace DevKid.src.Application.Dto.LessonDtos
{
    public class LessonCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid ChapterId { get; set; }
    }
}
