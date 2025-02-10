using AutoMapper;
using DevKid.src.Application.Dto.LessonDtos;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/lessons")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepo _lessonRepo;
        private readonly IMapper _mapper;
        private readonly IChapterRepo _chapterRepo;
        public LessonController(ILessonRepo lessonRepo, IMapper mapper, IChapterRepo chapterRepo)
        {
            _lessonRepo = lessonRepo;
            _mapper = mapper;
            _chapterRepo = chapterRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLessons()
        {
            var response = new ResponseDto();
            try
            {
                var lessons = await _lessonRepo.GetAllLessons();
                var mappedLessons = _mapper.Map<IEnumerable<LessonDto>>(lessons);
                if (mappedLessons != null && mappedLessons.Count() > 0)
                {
                    response.Message = "Lessons fetched successfully";
                    response.Result = new ResultDto { Data = mappedLessons };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "No lessons found";
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
        public async Task<IActionResult> GetLessonById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var lesson = await _lessonRepo.GetLessonById(id);
                var mappedLesson = _mapper.Map<LessonDto>(lesson);
                if (mappedLesson != null)
                {
                    response.Message = "Lesson fetched successfully";
                    response.Result = new ResultDto { Data = mappedLesson };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Lesson not found";
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
        public async Task<IActionResult> CreateLesson([FromBody] LessonCreateDto lessonCreateDto)
        {
            var response = new ResponseDto();
            try
            {
                var chapter = await _chapterRepo.GetChapterById(lessonCreateDto.ChapterId);
                var lesson = _mapper.Map<Lesson>(lessonCreateDto);
                var result = await _lessonRepo.AddLesson(lesson);
                if (result)
                {
                    response.Message = "Lesson added successfully";
                    response.IsSuccess = true;
                    return Created("", response);
                }
                else
                {
                    response.Message = "Lesson not added";
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
        public async Task<IActionResult> UpdateLesson(Guid id, [FromBody] LessonUpdateDto lesson)
        {
            var response = new ResponseDto();
            try
            {
                var lessonToUpdate = await _lessonRepo.GetLessonById(id);
                var mappedLesson = _mapper.Map(lesson, lessonToUpdate);
                var result = await _lessonRepo.UpdateLesson(mappedLesson);
                if (result)
                {
                    response.Message = "Lesson updated successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Lesson not updated";
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
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _lessonRepo.DeleteLesson(id);
                if (result)
                {
                    response.Message = "Lesson deleted successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Lesson not deleted";
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
