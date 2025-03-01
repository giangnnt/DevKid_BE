using DevKid.src.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using static DevKid.src.Domain.Entities.Material;

namespace DevKid.src.Application.Dto
{
    public class MaterialUploadDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public MaterialType Type { get; set; }
        public Guid LessonId { get; set; }
    }
    public class MaterialUploadLinkDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public MaterialType Type { get; set; } = MaterialType.Link;
        public Guid LessonId { get; set; }
        public string Url { get; set; } = null!;
    }
}
