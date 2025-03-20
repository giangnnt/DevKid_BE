using AutoMapper;
using DevKid.src.Domain.Entities;
using System.Text.Json.Serialization;
using static DevKid.src.Domain.Entities.StudentQuiz;

namespace DevKid.src.Application.Dto
{
    public class StudentQuizDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Dictionary<string, StudentAns>? QuesAns { get; set; }
        public float Score { get; set; } = 0;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuizStatus Status { get; set; }
    }
    public class StudentQuizCreateDto
    {
        public Guid QuizId { get; set; }
        public Dictionary<string, List<Guid>?>? QuesAns { get; set; }
    }
    public class StudentQuizUpdateDto
    {
        public Guid QuizId { get; set; }
        public Dictionary<string, List<Guid>?>? QuesAns { get; set; }
    }
    public class StudentQuizProfile : Profile
    {
        public StudentQuizProfile()
        {
            CreateMap<StudentQuizCreateDto, StudentQuiz>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Student, opts => opts.Ignore())
                .ForMember(dest => dest.QuesAns, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<StudentQuizUpdateDto, StudentQuiz>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Student, opts => opts.Ignore())
                .ForMember(dest => dest.QuesAns, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<StudentQuiz, StudentQuizDto>()
                .ForMember(dest => dest.QuesAns, opts => opts.MapFrom(src => src.QuesAns))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
