using AutoMapper;
using DevKid.src.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static DevKid.src.Domain.Entities.Payment;

namespace DevKid.src.Application.Dto
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public float Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string PaymentMethod { set; get; } = null!;
        public StatusEnum Status;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
    public class PaymentCreateDto
    {
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        [Range(1, float.MaxValue)]
        public float Amount { get; set; }
    }
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PaymentCreateDto, Payment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
