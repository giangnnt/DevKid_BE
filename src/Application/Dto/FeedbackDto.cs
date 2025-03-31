using AutoMapper;
using DevKid.src.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DevKid.src.Application.Dto
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
        public string? Description { get; set; }
        public float Rating { get; set; }
    }
    public class FeedbackCreateDto
    {
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        public Guid StudentId { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(1, 5)]
        public float Rating { get; set; }
    }
    public class FeedbackUpdateDto
    {
        public string? Description { get; set; }
        [Range(1, 5)]
        public float? Rating { get; set; }
    }
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<FeedbackCreateDto, Feedback>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<FeedbackUpdateDto, Feedback>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
