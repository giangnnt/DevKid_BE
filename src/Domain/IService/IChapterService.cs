using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IService
{
    public interface IChapterService
    {
        Task<ResponseDto> GetAllChapters();
        Task<ResponseDto> GetChapterById(Guid id);
        Task<ResponseDto> AddChapter(Chapter chapter);
        Task<ResponseDto> UpdateChapter(Chapter chapter);
        Task<ResponseDto> DeleteChapter(Guid id);
    }
}
