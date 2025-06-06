﻿using AutoMapper;
using DevKid.src.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static DevKid.src.Domain.Entities.Material;

namespace DevKid.src.Application.Dto
{
    public class MaterialDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MaterialType Type { get; set; }
        public string? Url { get; set; }
        public Guid LessonId { get; set; }
    }
    public class MaterialCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MaterialType Type { get; set; }
        public string? Url { get; set; }
        [Required]
        public Guid LessonId { get; set; }
    }
    public class MaterialUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class MaterialProfile : Profile
    {
        public MaterialProfile()
        {
            CreateMap<Material, MaterialDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<MaterialCreateDto, Material>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<MaterialUpdateDto, Material>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore())
                .ForMember(dest => dest.Url, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
