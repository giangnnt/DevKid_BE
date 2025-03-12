using AutoMapper;
using DevKid.src.Domain.Entities;
using static DevKid.src.Domain.Entities.StudentQuiz;

namespace DevKid.src.Application.Dto
{
    public class StudentQuizDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid AnsId { get; set; }
        public AnsDto Ans { get; set; } = null!;
        public QuizStatus Status { get; set; }
    }
    public class StudentQuizCreateDto
    {
        public Guid StudentId { get; set; }
        public Guid AnsId { get; set; }
    }
    public class StudentQuizUpdateDto
    {
        public Guid AnsId { get; set; }
    }
    public class StudentQuizProfile : Profile
    {
        public StudentQuizProfile()
        {
            CreateMap<StudentQuizCreateDto, StudentQuiz>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Student, opts => opts.Ignore())
                .ForMember(dest => dest.Ans, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<StudentQuizUpdateDto, StudentQuiz>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Student, opts => opts.Ignore())
                .ForMember(dest => dest.Ans, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<StudentQuiz, StudentQuizDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
