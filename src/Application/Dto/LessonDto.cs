using AutoMapper;
using DevKid.src.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DevKid.src.Application.Dto
{
    public class LessonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid ChapterId { get; set; }
        public List<MaterialDto> Materials { get; set; } = new();
        public List<CommentDto> Comments { get; set; } = new();
        public List<QuizDto> Quizzes { get; set; } = new();
    }
    public class LessonCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        public Guid ChapterId { get; set; }
    }
    public class LessonUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            CreateMap<Lesson, LessonDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LessonCreateDto, Lesson>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Chapter, opts => opts.Ignore())
                .ForMember(dest => dest.Materials, opts => opts.Ignore())
                .ForMember(dest => dest.Comments, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<LessonUpdateDto, Lesson>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Chapter, opts => opts.Ignore())
                .ForMember(dest => dest.Materials, opts => opts.Ignore())
                .ForMember(dest => dest.Comments, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
