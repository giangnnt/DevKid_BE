using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface IFeedbackRepo
    {
        Task<IEnumerable<Feedback>> GetFeedbacks();
        Task<Feedback> GetFeedback(Guid id);
        Task<bool> AddFeedback(Feedback feedback);
        Task<bool> UpdateFeedback(Feedback feedback);
        Task<bool> DeleteFeedback(Guid id);
        Task<IEnumerable<Feedback>> GetFeedbacksByCourseId(Guid courseId);
        Task<bool> HaveUserFeedbackOnCourse(Guid courseId, Guid userId);
    }
}
