using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevKid.src.Domain.Entities
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public string? QuesAnsJson { get; set; }
        [NotMapped]
        public Dictionary<string, List<Ans>> QuesAns
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, List<Ans>>>(QuesAnsJson ?? "") ?? new Dictionary<string, List<Ans>>();
            set => QuesAnsJson = JsonConvert.SerializeObject(value);
        }
        public Lesson Lesson { get; set; } = null!;

    }
    public class Ans
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
    }
}
