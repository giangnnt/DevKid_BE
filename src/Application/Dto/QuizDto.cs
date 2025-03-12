using AutoMapper;
using DevKid.src.Domain.Entities;
using Newtonsoft.Json;

namespace DevKid.src.Application.Dto
{
    public class QuizDto
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public Dictionary<string, List<AnsDto>> QuesAns = null!;
    }
    public class QuizAdminDto
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public Dictionary<string, List<Ans>> QuesAns = null!;
    }
    public class QuizCreateDto
    {
        public Guid LessonId { get; set; }
        public Dictionary<string, List<Ans>>? QuesAns { get; set; }
    }
    public class QuizUpdateDto
    {
        public Dictionary<string, List<Ans>>? QuesAns { get; set; }
    }
    public class QuizProfile : Profile
    {
        public QuizProfile()
        {
            CreateMap<Quiz, QuizDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<QuizCreateDto, Quiz>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Lesson, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<QuizUpdateDto, Quiz>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.LessonId, opts => opts.Ignore())
                .ForMember(dest => dest.Lesson, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Quiz, QuizAdminDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
