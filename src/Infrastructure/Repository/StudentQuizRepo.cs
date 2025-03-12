using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class StudentQuizRepo : IStudentQuizRepo
    {
        private readonly DevKidContext _context;
        public StudentQuizRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateStudentQuiz(StudentQuiz studentQuiz)
        {
            _context.StudentQuizzes.Add(studentQuiz);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteStudentQuiz(Guid id)
        {
            _context.StudentQuizzes.Remove(await GetStudentQuizById(id));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<StudentQuiz> GetStudentQuizById(Guid id)
        {
            return await _context.StudentQuizzes.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("StudentQuiz not found");
        }

        public async Task<IEnumerable<StudentQuiz>> GetStudentQuizByStudentIdLessonId(Guid studentId, Guid lessonId)
        {
            return await _context.StudentQuizzes
                .Include(x => x.Ans)
                .Where(x => x.StudentId == studentId && x.Ans.Quiz.LessonId == lessonId)
                .ToListAsync();
        }

        public async Task<StudentQuiz> GetStudentQuizByStudentIdQuizId(Guid studentId, Guid quizId)
        {
            return await _context.StudentQuizzes
                .Include(x => x.Ans)
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.Ans.QuizId == quizId) ?? throw new Exception("Student answer for this quiz not found");
        }

        public async Task<bool> UpdateStudentQuiz(StudentQuiz studentQuiz)
        {
            _context.StudentQuizzes.Update(studentQuiz);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
