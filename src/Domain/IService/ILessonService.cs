using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IService
{
    public interface ILessonService
    {
        Task<ResponseDto> GetAllLessons();
        Task<ResponseDto> GetLessonById(Guid id);
        Task<ResponseDto> AddLesson(Lesson lesson);
        Task<ResponseDto> UpdateLesson(Lesson lesson);
        Task<ResponseDto> DeleteLesson(Guid id);
    }
}
