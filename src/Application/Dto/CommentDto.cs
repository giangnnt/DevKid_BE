using AutoMapper;
using DevKid.src.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DevKid.src.Application.Dto
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public Guid StudentId { get; set; }
        public Guid LessonId { get; set; }
    }
    public class CommentCreateDto
    {
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public Guid LessonId { get; set; }
    }
    public class CommentUpdateDto
    {
        public string? Content { get; set; }
    }
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CommentCreateDto, Comment>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Student, opts => opts.Ignore())
                .ForMember(dest => dest.Lesson, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CommentUpdateDto, Comment>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Student, opts => opts.Ignore())
                .ForMember(dest => dest.Lesson, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
