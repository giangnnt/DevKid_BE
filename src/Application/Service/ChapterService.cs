using AutoMapper;
using DevKid.src.Application.Dto.ChapterDtos;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Domain.IService;

namespace DevKid.src.Application.Service
{
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepo _chapterRepo;
        private readonly IMapper _mapper;
        public ChapterService(IChapterRepo chapterRepo, IMapper mapper)
        {
            _chapterRepo = chapterRepo;
            _mapper = mapper;
        }
        public async Task<ResponseDto> AddChapter(ChapterCreateDto chapter)
        {
            var response = new ResponseDto();
            try
            {
                var mappedChapter = _mapper.Map<Chapter>(chapter);
                var result = await _chapterRepo.AddChapter(mappedChapter);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Chapter added successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Chapter not added";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> DeleteChapter(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _chapterRepo.DeleteChapter(id);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Chapter deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Chapter not deleted";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetAllChapters()
        {
            var response = new ResponseDto();
            try
            {
                var chapters = await _chapterRepo.GetAllChapters();
                if (chapters != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Chapters fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<ChapterDto>>(chapters)
                    };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Chapters not fetched";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetChapterById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var chapter = await _chapterRepo.GetChapterById(id);
                if (chapter != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Chapter fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<ChapterDto>(chapter)
                    };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Chapter not fetched";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> UpdateChapter(Guid id, ChapterUpdateDto chapter)
        {
            var response = new ResponseDto();
            try
            {
                var chapterToUpdate = await _chapterRepo.GetChapterById(id);
                var mappedChapter = _mapper.Map(chapter, chapterToUpdate);
                var result = await _chapterRepo.UpdateChapter(mappedChapter);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Chapter updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Chapter not updated";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
    }
}
