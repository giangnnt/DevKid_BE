using AutoMapper;
using DevKid.src.Application.Dto.ChapterDtos;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/chapters")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly IChapterRepo _chapterRepo;
        private readonly ICourseRepo _courseRepo;
        private readonly IMapper _mapper;
        public ChaptersController(IChapterRepo chapterRepo, IMapper mapper, ICourseRepo courseRepo)
        {
            _chapterRepo = chapterRepo;
            _mapper = mapper;
            _courseRepo = courseRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllChapters()
        {
            var response = new ResponseDto();
            try
            {
                var chapters = await _chapterRepo.GetAllChapters();
                if (chapters != null)
                {
                    response.Message = "Chapters fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<ChapterDto>>(chapters)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Chapters not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapterById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var chapter = await _chapterRepo.GetChapterById(id);
                if (chapter != null)
                {
                    response.Message = "Chapter fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<ChapterDto>(chapter)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Chapter not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddChapter([FromBody] ChapterCreateDto chapter)
        {
            var response = new ResponseDto();
            try
            {
                var course = await _courseRepo.GetCourseById(chapter.CourseId);
                var mappedChapter = _mapper.Map<Chapter>(chapter);
                mappedChapter.Course = course;
                var result = await _chapterRepo.AddChapter(mappedChapter);
                if (result)
                {
                    response.Message = "Chapter added successfully";
                    response.IsSuccess = true;
                    return Created("", response);
                }
                else
                {
                    response.Message = "Chapter not added";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChapter(Guid id, ChapterUpdateDto chapter)
        {
            var response = new ResponseDto();
            try
            {
                var chapterToUpdate = await _chapterRepo.GetChapterById(id);
                var mappedChapter = _mapper.Map(chapter, chapterToUpdate);
                var result = await _chapterRepo.UpdateChapter(mappedChapter);
                if (result)
                {
                    response.Message = "Chapter updated successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Chapter not updated";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _chapterRepo.DeleteChapter(id);
                if (result)
                {
                    response.Message = "Chapter deleted successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Chapter not deleted";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
