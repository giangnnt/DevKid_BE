using AutoMapper;
using DevKid.src.Domain.Entities;

namespace DevKid.src.Application.Dto
{
    public class AnsDto
    {
        public string? Content { get; set; }
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
    }
    public class AnsCreateDto
    {
        public string? Content { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuizId { get; set; }
    }
    public class AnsUpdateDto
    {
        public string? Content { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuizId { get; set; }
    }
    public class AnsProfile : Profile
    {
        public AnsProfile()
        {
            CreateMap<AnsCreateDto, Ans>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Quiz, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AnsUpdateDto, Ans>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Quiz, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Ans, AnsDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
