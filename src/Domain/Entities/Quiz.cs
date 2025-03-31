using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevKid.src.Domain.Entities
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public string? QuesAnsJson { get; set; }
        [NotMapped]
        public Dictionary<string, List<Ans>>? QuesAns
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, List<Ans>>>(QuesAnsJson ?? "");
            set => QuesAnsJson = JsonConvert.SerializeObject(value);
        }
        public Lesson Lesson { get; set; } = null!;
    }
    [NotMapped]
    public class Ans
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public bool IsCorrect { get; set; }
    }
    //[NotMapped]
    //public class Ques
    //{
    //    public Guid Id { get; set; } = Guid.NewGuid();
    //    public string? Content { get; set; }
    //}
}
