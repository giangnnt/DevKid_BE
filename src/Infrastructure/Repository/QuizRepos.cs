using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class QuizRepo : IQuizRepo
    {
        private readonly DevKidContext _context;
        public QuizRepo(DevKidContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAns(Ans ans)
        {
            _context.Answers.Add(ans);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateQuiz(Quiz quiz) 
        {

            _context.Quizzes.Add(quiz);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAns(Guid id)
        {
            _context.Answers.Remove(await GetAnsById(id));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteQuiz(Guid id)
        {
            _context.Quizzes.Remove(await GetQuizById(id));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Ans> GetAnsById(Guid id)
        {
            return await _context.Answers.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Ans not found");
        }

        public async Task<Quiz> GetQuizById(Guid id)
        {
            return await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Quiz not found");
            
        }

        public async Task<IEnumerable<Quiz>> GetQuizByLessonId(Guid lessonId)
        {
            return await _context.Quizzes.Where(x => x.LessonId == lessonId).ToListAsync();
        }

        public async Task<bool> UpdateAns(Ans ans)
        {
            _context.Answers.Update(ans);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateQuiz(Quiz quiz)
        {
            _context.Quizzes.Update(quiz);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
