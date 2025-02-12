using AutoMapper;
using DevKid.src.Domain.Entities;

namespace DevKid.src.Application.Dto
{
    public class ChapterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid CourseId { get; set; }
        public List<Lesson> Lessons { get; set; } = new();
    }
    public class ChapterCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid CourseId { get; set; }
    }
    public class ChapterUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class ChapterProfile : Profile
    {
        public ChapterProfile()
        {
            CreateMap<Chapter, ChapterDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ChapterCreateDto, Chapter>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Course, opts => opts.Ignore())
                .ForMember(dest => dest.Lessons, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ChapterUpdateDto, Chapter>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.CourseId, opts => opts.Ignore())
                .ForMember(dest => dest.Course, opts => opts.Ignore())
                .ForMember(dest => dest.Lessons, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
