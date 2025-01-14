using DevKid.src.Application.Dto.CourseDtos;
using DevKid.src.Domain.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var response = await _courseService.GetAllCourses();
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var response = await _courseService.GetCourseById(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseCreateDto course)
        {
            var response = await _courseService.AddCourse(course);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseUpdateDto course)
        {
            var response = await _courseService.UpdateCourse(course);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var response = await _courseService.DeleteCourse(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
