using AutoMapper;

namespace DevKid.src.Application.Dto.CourseDtos
{
    public class CourseCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public float Price { get; set; }
    }
}
