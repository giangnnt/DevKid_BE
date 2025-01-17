using DevKid.src.Application.Dto.CourseDtos;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IService
{
    public interface ICourseService
    {
        Task<ResponseDto> GetAllCourses();
        Task<ResponseDto> GetCourseById(Guid id);
        Task<ResponseDto> AddCourse(CourseCreateDto course);
        Task<ResponseDto> UpdateCourse(Guid id, CourseUpdateDto course);
        Task<ResponseDto> DeleteCourse(Guid id);
    }
}
