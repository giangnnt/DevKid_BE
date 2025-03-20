using AutoMapper;
using DevKid.src.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DevKid.src.Application.Dto
{
    public class AnsDto
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
    }
    public class AnsAdminDto
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public bool IsCorrect { get; set; }
    }
    public class AnsCreateDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Content { get; set; }
        [Required]
        public bool IsCorrect { get; set; } = false;
    }
    public class AnsUpdateDto
    {
        public string? Content { get; set; }
        public bool? IsCorrect { get; set; }
    }
    public class AnsProfile : Profile
    {
        public AnsProfile()
        {
            CreateMap<AnsCreateDto, Ans>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AnsUpdateDto, Ans>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Ans, AnsDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Ans, AnsAdminDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
