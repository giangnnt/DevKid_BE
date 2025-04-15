using AutoMapper;
using DevKid.src.Application.Constant;
using DevKid.src.Application.Core;
using DevKid.src.Application.Dto;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Middleware;
using DevKid.src.Application.Service;
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
        private readonly IBoughtCertificateService _boughtCertificateService;
        public ChaptersController(IChapterRepo chapterRepo, IMapper mapper, ICourseRepo courseRepo, IBoughtCertificateService boughtCertificateService)
        {
            _chapterRepo = chapterRepo;
            _mapper = mapper;
            _courseRepo = courseRepo;
            _boughtCertificateService = boughtCertificateService;
        }
        //[HttpGet]
        //public async Task<IActionResult> GetAllChapters()
        //{
        //    var response = new ResponseDto();
        //    try
        //    {
        //        var chapters = await _chapterRepo.GetAllChapters();
        //        if (chapters != null)
        //        {
        //            response.Message = "Chapters fetched successfully";
        //            response.Result = new ResultDto
        //            {
        //                Data = _mapper.Map<IEnumerable<ChapterDto>>(chapters)
        //            };
        //            response.IsSuccess = true;
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            response.Message = "Chapters not fetched";
        //            response.IsSuccess = false;
        //            return BadRequest(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //        response.IsSuccess = false;
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}
        [Protected]
        [HttpGet("{id}")]
        [Permission(PermissionSlug.CHAPTER_ALL, PermissionSlug.CHAPTER_VIEW)]
        public async Task<IActionResult> GetChapterById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var payload = HttpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
                var chapter = await _chapterRepo.GetChapterById(id);
                if (chapter != null)
                {
                    // only admin and manager can access chapter detail without buying
                    if (payload.RoleId != RoleConst.ADMIN_ID && payload.RoleId != RoleConst.MANAGER_ID)
                    {
                        if (!await _boughtCertificateService.CheckCertificateAsync(chapter.Id, payload.UserId))
                        {
                            response.Message = "Unauthorized";
                            response.IsSuccess = false;
                            return BadRequest(response);
                        }
                    }
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
        [Protected]
        [HttpPost]
        [Permission(PermissionSlug.CHAPTER_ALL)]
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
        [Protected]
        [HttpPut("{id}")]
        [Permission(PermissionSlug.CHAPTER_ALL)]
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
        [Protected]
        [HttpDelete("{id}")]
        [Permission(PermissionSlug.CHAPTER_ALL)]
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
        [Protected]
        [HttpGet("course/{courseId}")]
        [Permission(PermissionSlug.CHAPTER_ALL, PermissionSlug.CHAPTER_VIEW)]
        public async Task<IActionResult> GetChaptersByCourseId(Guid courseId)
        {
            var response = new ResponseDto();
            try
            {
                // only admin and manager can access chapter detail without buying
                var payload = HttpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
                if (payload.RoleId != RoleConst.ADMIN_ID && payload.RoleId != RoleConst.MANAGER_ID)
                {
                    if (!await _boughtCertificateService.CheckCertificateAsync(courseId, payload.UserId))
                    {
                        response.Message = "Unauthorized";
                        response.IsSuccess = false;
                        return BadRequest(response);
                    }
                }
                var chapters = await _chapterRepo.GetChaptersByCourseId(courseId);
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
        [Protected]
        [HttpPost("{courserId}")]
        [Permission(PermissionSlug.CHAPTER_ALL)]
        public async Task<IActionResult> AddListChapter([FromBody] List<ChapterCreateDto> chapters, Guid courserId)
        {
            var response = new ResponseDto();
            try
            {
                var result = false;
                var course = await _courseRepo.GetCourseById(courserId);
                foreach (var chapter in chapters)
                {
                    var mappedChapter = _mapper.Map<Chapter>(chapter);
                    mappedChapter.Course = course;
                    result = await _chapterRepo.AddChapter(mappedChapter);
                    if (!result)
                    {
                        response.Message = "Chapter not added";
                        response.IsSuccess = false;
                        return BadRequest(response);
                    }
                }
                response.Message = "Chapter added successfully";
                response.IsSuccess = true;
                return Created("", response);

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
