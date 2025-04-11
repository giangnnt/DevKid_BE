using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DevKid.src.Infrastructure.Repository
{
    public class CourseRepo : ICourseRepo
    {
        private readonly DevKidContext _context;
        public CourseRepo(DevKidContext context)
        {
            _context = context;
        }
        public async Task<bool> AddCourse(Course course)
        {
            _context.Courses.Add(course);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCourse(Guid id)
        {
            _context.Courses.Remove(_context.Courses.Find(id) ?? throw new Exception("Course not found"));
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Courses
               .Include(x => x.Chapters)
               .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetBoughtCourse(Guid userId)
        {
            var orders = _context.Orders
                .Include(x => x.Course)
                .Where(x => x.StudentId == userId && x.Status == Order.StatusEnum.Completed);
            var courses = await orders.Select(x => x.Course).ToListAsync();
            return courses;
        }

        public async Task<Course> GetCourseById(Guid id)
        {
            return await _context.Courses
                .Include(x => x.Chapters)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Course not found");
        }

        public async Task<bool> UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
