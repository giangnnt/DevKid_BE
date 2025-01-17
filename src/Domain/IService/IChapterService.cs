using DevKid.src.Application.Dto.ChapterDtos;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;

namespace DevKid.src.Domain.IService
{
    public interface IChapterService
    {
        Task<ResponseDto> GetAllChapters();
        Task<ResponseDto> GetChapterById(Guid id);
        Task<ResponseDto> AddChapter(ChapterCreateDto chapter);
        Task<ResponseDto> UpdateChapter(Guid id, ChapterUpdateDto chapter);
        Task<ResponseDto> DeleteChapter(Guid id);
    }
}
