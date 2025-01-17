using DevKid.src.Application.Dto.ChapterDtos;
using DevKid.src.Domain.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/chapters")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly IChapterService _chapterService;
        public ChapterController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllChapters()
        {
            var response = await _chapterService.GetAllChapters();
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapterById(Guid id)
        {
            var response = await _chapterService.GetChapterById(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> AddChapter([FromBody] ChapterCreateDto chapter)
        {
            var response = await _chapterService.AddChapter(chapter);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChapter(Guid id, ChapterUpdateDto chapter)
        {
            var response = await _chapterService.UpdateChapter(id, chapter);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(Guid id)
        {
            var response = await _chapterService.DeleteChapter(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
