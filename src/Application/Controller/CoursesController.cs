using AutoMapper;
using DevKid.src.Application.Dto.CourseDtos;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepo _courseRepo;
        private readonly IMapper _mapper;
        public CoursesController(ICourseRepo courseRepo, IMapper mapper)
        {
            _courseRepo = courseRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var response = new ResponseDto();
            try
            {
                var courses = await _courseRepo.GetAllCourses();
                var mappedCourses = _mapper.Map<IEnumerable<CourseDto>>(courses);
                if (mappedCourses != null && mappedCourses.Count() > 0)
                {
                    response.Message = "Courses fetched successfully";
                    response.Result = new ResultDto { Data = mappedCourses };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "No courses found";
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
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var course = await _courseRepo.GetCourseById(id);
                var mappedCourse = _mapper.Map<CourseDto>(course);
                if (mappedCourse != null)
                {
                    response.Message = "Course fetched successfully";
                    response.Result = new ResultDto { Data = mappedCourse };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Course not found";
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
        public async Task<IActionResult> AddCourse([FromBody] CourseCreateDto course)
        {
            var response = new ResponseDto();
            try
            {
                var mappedCourse = _mapper.Map<Course>(course);
                var result = await _courseRepo.AddCourse(mappedCourse);
                if (result)
                {
                    response.Message = "Course added successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Course not added";
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
        public async Task<IActionResult> UpdateCourse(Guid id, CourseUpdateDto course)
        {
            var response = new ResponseDto();
            try
            {
                var courseToUpdate = await _courseRepo.GetCourseById(id);
                var mappedCourse = _mapper.Map(course, courseToUpdate);
                var result = await _courseRepo.UpdateCourse(mappedCourse);
                if (result)
                {
                    response.Message = "Course updated successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Course not updated";
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
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _courseRepo.DeleteCourse(id);
                if (result)
                {
                    response.Message = "Course deleted successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Course not deleted";
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
