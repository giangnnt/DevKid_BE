using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface IChapterRepo
    {
        Task<IEnumerable<Chapter>> GetAllChapters();
        Task<Chapter> GetChapterById(Guid id);
        Task<bool> AddChapter(Chapter chapter);
        Task<bool> UpdateChapter(Chapter chapter);
        Task<bool> DeleteChapter(Guid id);
        Task<IEnumerable<Chapter>> GetChaptersByCourseId(Guid courseId);
    }
}
