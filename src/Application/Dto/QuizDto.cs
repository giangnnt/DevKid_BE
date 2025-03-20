using AutoMapper;
using DevKid.src.Domain.Entities;
using Newtonsoft.Json;

namespace DevKid.src.Application.Dto
{
    public class QuizDto
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public Dictionary<string, List<AnsDto>>? QuesAns { get; set; }
    }
    public class QuizAdminDto
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public Dictionary<string, List<AnsAdminDto>>? QuesAns { get; set; }
    }
    public class QuizCreateDto
    {
        public Guid LessonId { get; set; }
        public Dictionary<string, List<AnsCreateDto>>? QuesAns { get; set; }
    }
    public class QuizUpdateDto
    {
        public Dictionary<string, List<AnsUpdateDto>>? QuesAns { get; set; }
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
                .ForMember(dest => dest.QuesAnsJson, opts => opts.MapFrom(src => JsonConvert.SerializeObject(src.QuesAns)))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<QuizUpdateDto, Quiz>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.LessonId, opts => opts.Ignore())
                .ForMember(dest => dest.Lesson, opts => opts.Ignore())
                .ForMember(dest => dest.QuesAnsJson, opts => opts.MapFrom(src => JsonConvert.SerializeObject(src.QuesAns)))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Quiz, QuizAdminDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
