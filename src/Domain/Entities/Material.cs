﻿namespace DevKid.src.Domain.Entities
{
    public class Material
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public enum MaterialType
        {
            Image,
            Video,
            Doc,
            Link
        }
        public MaterialType Type { get; set; }
        public string? Url { get; set; }
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
