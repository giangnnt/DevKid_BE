using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using static DevKid.src.Domain.Entities.Quiz;

namespace DevKid.src.Domain.Entities
{
    public class StudentQuiz
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string? QuesAnsJson { get; set; }
        [NotMapped]
        public Dictionary<string, StudentAns>? QuesAns
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, StudentAns>>(QuesAnsJson ?? "");
            set => QuesAnsJson = JsonConvert.SerializeObject(value);
        }
        public Student Student { get; set; } = null!;
        public Guid QuizId { get; set; }
        public float Score { get; set; } = 0;
        public enum QuizStatus
        {
            Uncompleted,
            Completed   
        }
        public QuizStatus Status { get; set; } = QuizStatus.Uncompleted;
    }
    [NotMapped]
    public class StudentAns
    {
        public List<Guid>? Id { get; set; }
        public bool IsCorrect { get; set; }
    }
}
