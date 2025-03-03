using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface ILessonRepo
    {
        Task<IEnumerable<Lesson>> GetAllLessons();
        Task<Lesson> GetLessonById(Guid id);
        Task<bool> AddLesson(Lesson lesson);
        Task<bool> UpdateLesson(Lesson lesson);
        Task<bool> DeleteLesson(Guid id);
        Task<IEnumerable<Lesson>> GetLessonsByChapterId(Guid chapterId);
    }
}
