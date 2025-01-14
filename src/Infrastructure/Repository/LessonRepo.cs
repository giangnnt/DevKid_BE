using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class LessonRepo : ILessonRepo
    {
        private readonly DevKidContext _context;
        public LessonRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> AddLesson(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteLesson(Guid id)
        {
            _context.Lessons.Remove(_context.Lessons.Find(id) ?? throw new Exception("Lesson not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Lesson>> GetAllLessons()
        {
            return await _context.Lessons
                .Include(x => x.Materials)
                .ToListAsync();
        }

        public async Task<Lesson> GetLessonById(Guid id)
        {
            return await _context.Lessons
                .Include(x => x.Materials)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Lesson not found");
        }

        public async Task<bool> UpdateLesson(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
