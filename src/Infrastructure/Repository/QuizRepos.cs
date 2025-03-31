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

        //public async Task<List<Guid>> CreateListQuiz(List<Quiz> quizzes)
        //{
        //    foreach (var quiz in quizzes)
        //    {
        //        _context.Quizzes.Add(quiz);
        //    }
        //    await _context.SaveChangesAsync();
        //    return quizzes.Select(x => x.Id).ToList();
        //}

        public async Task<bool> CreateQuiz(Quiz quiz)
        {

            _context.Quizzes.Add(quiz);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteQuiz(Guid id)
        {
            _context.Quizzes.Remove(await GetQuizById(id));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Quiz> GetQuizById(Guid id)
        {
            return await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Quiz not found");

        }

        public async Task<IEnumerable<Quiz>> GetQuizByLessonId(Guid lessonId)
        {
            return await _context.Quizzes.Where(x => x.LessonId == lessonId).ToListAsync();
        }

        public async Task<bool> UpdateQuiz(Quiz quiz)
        {
            _context.Quizzes.Update(quiz);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
