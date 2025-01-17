using System.ComponentModel.DataAnnotations;

namespace DevKid.src.Domain.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public Guid StudentId { get; set; }
        public Guid LessonId { get; set; }

    }
}
