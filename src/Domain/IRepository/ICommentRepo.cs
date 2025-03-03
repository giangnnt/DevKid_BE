using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface ICommentRepo
    {
        Task<IEnumerable<Comment>> GetComments();
        Task<Comment> GetComment(Guid id);
        Task<bool> AddComment(Comment comment);
        Task<bool> UpdateComment(Comment comment);
        Task<bool> DeleteComment(Guid id);
        Task<IEnumerable<Comment>> GetCommentsByLessonId(Guid lessonId);
    }
}
