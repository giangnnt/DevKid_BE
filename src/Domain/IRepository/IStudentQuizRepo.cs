using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface IStudentQuizRepo
    {
        Task<StudentQuiz> GetStudentQuizById(Guid id);
        Task<IEnumerable<StudentQuiz>> GetStudentQuizByStudentIdLessonId(Guid studentId, Guid lessonId); // tính điểm
        Task<bool> CreateStudentQuiz(StudentQuiz studentQuiz);
        Task<bool> UpdateStudentQuiz(StudentQuiz studentQuiz);
        Task<bool> DeleteStudentQuiz(Guid id);
        Task<StudentQuiz> GetStudentQuizByStudentIdQuizId(Guid studentId, Guid quizId);
    }
}
