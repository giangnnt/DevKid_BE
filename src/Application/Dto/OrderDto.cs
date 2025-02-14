﻿using AutoMapper;
using DevKid.src.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static DevKid.src.Domain.Entities.Order;

namespace DevKid.src.Application.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public float Price { get; set; }
        public StatusEnum Status;
    }
    public class OrderCreateDto
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        [Range(1, float.MaxValue)]
        public float Price { get; set; }
    }
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
