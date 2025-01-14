using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class ChapterRepo : IChapterRepo
    {
        private readonly DevKidContext _context;
        public ChapterRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> AddChapter(Chapter chapter)
        {
            _context.Chapters.Add(chapter);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteChapter(Guid id)
        {
            _context.Chapters.Remove(_context.Chapters.Find(id) ?? throw new Exception("Chapter not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Chapter>> GetAllChapters()
        {
            return await _context.Chapters
                .Include(x => x.Lessons)
                .ToListAsync();
        }

        public async Task<Chapter> GetChapterById(Guid id)
        {
            return await _context.Chapters
                .Include(x => x.Lessons)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Chapter not found");
        }

        public async Task<bool> UpdateChapter(Chapter chapter)
        {
            _context.Chapters.Update(chapter);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
