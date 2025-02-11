using System.ComponentModel.DataAnnotations;
using DevKid.src.Infrastructure.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Domain.Entities
{
    public class Material
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public enum MaterialType
        {
            Video,
            Document,
            Link
        }
        public MaterialType Type { get; set; }
        public string? Url { get; set; }
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
