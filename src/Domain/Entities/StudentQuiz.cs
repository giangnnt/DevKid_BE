using System.ComponentModel.DataAnnotations.Schema;
using static DevKid.src.Domain.Entities.Quiz;

namespace DevKid.src.Domain.Entities
{
    public class StudentQuiz
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid AnsId { get; set; }
        public Ans Ans { get; set; } = null!;
        public Student Student { get; set; } = null!;
        public enum QuizStatus
        {
            InProgress,
            Correct,
            Incorrect
        }
        public QuizStatus Status { get; set; } = QuizStatus.InProgress;
    }
}
