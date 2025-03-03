using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IRepository
{
    public interface ICourseRepo
    {
        Task<IEnumerable<Course>> GetAllCourses();
        Task<Course> GetCourseById(Guid id);
        Task<bool> AddCourse(Course course);
        Task<bool> UpdateCourse(Course course);
        Task<bool> DeleteCourse(Guid id);
        Task<IEnumerable<Course>> GetBoughtCourse(Guid userId);
    }
}
