using static DevKid.src.Domain.Entities.Material;

namespace DevKid.src.Application.Dto.MaterialDtos
{
    public class MaterialCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public MaterialType Type { get; set; }
        public string? Url { get; set; }
        public Guid LessonId { get; set; }
    }
}
