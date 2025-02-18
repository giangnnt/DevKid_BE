using AutoMapper;
using DevKid.src.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static DevKid.src.Domain.Entities.Payment;

namespace DevKid.src.Application.Dto
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public long OrderId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string PaymentMethod { set; get; } = null!;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusEnum Status { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
    public class PaymentCreateDto
    {
        [Required]
        public long OrderId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
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
