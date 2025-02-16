using AutoMapper;
using DevKid.src.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DevKid.src.Application.Dto
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public float Price { get; set; }
        public List<ChapterDto> Chapters { get; set; } = new();
    }
    public class CourseCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [Range(2000, 1000000)]
        public float Price { get; set; }
    }
    public class CourseUpdateDto
    {

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        [Range(2000, float.MaxValue)]
        public float? Price { get; set; }
    }
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CourseCreateDto, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Chapters, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CourseUpdateDto, Course>()
                .ForMember(dest => dest.Chapters, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
