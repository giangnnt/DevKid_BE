using AutoMapper;
using DevKid.src.Application.Constant;
using DevKid.src.Application.Core;
using DevKid.src.Application.Dto;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Middleware;
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
        private readonly IOrderRepo _orderRepo;
        public CoursesController(ICourseRepo courseRepo, IMapper mapper, IOrderRepo orderRepo)
        {
            _courseRepo = courseRepo;
            _mapper = mapper;
            _orderRepo = orderRepo;
        }
        // get course for user
        [HttpGet("summary")]
        public async Task<IActionResult> GetCourseUser()
        {
            var response = new ResponseDto();
            try
            {
                var courses = await _courseRepo.GetAllCourses();
                var activeCourses = courses.Where(c => c.Status == Course.CourseStatus.Active);
                var mappedCourses = _mapper.Map<IEnumerable<CourseAllDto>>(activeCourses);
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
        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetCourseByIdSummary(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var course = await _courseRepo.GetCourseById(id);
                var mappedCourse = _mapper.Map<CourseAllDto>(course);
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
        // get course for admin and manager
        [Protected]
        [HttpGet("detail")]
        [Permission(PermissionSlug.COURSE_ALL)]
        public async Task<IActionResult> GetCourses()
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
        // must bought course before access
        [Protected]
        [Permission(PermissionSlug.COURSE_ALL, PermissionSlug.COURSE_VIEW)]
        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetCourseById(Guid id)
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
                // only admin and manager can access course detail without buying
                if (payload.RoleId != RoleConst.GetRoleId("ADMIN") && payload.RoleId != RoleConst.GetRoleId("MANAGER"))
                {
                    if (!await _orderRepo.HaveUserBoughtCourse(id, payload.UserId))
                    {
                        response.Message = "Course not bought";
                        response.IsSuccess = false;
                        return BadRequest(response);
                    }
                }
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
        [Protected]
        [Permission(PermissionSlug.COURSE_ALL)]
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
        [Protected]
        [Permission(PermissionSlug.COURSE_ALL)]
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
        [Protected]
        [Permission(PermissionSlug.COURSE_ALL)]
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
        [Protected]
        [HttpGet("bought")]
        [Permission(PermissionSlug.COURSE_ALL, PermissionSlug.COURSE_VIEW)]
        public async Task<IActionResult> GetBoughtCourses()
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
                var courses = await _courseRepo.GetBoughtCourse(payload.UserId);
                var mappedCourses = _mapper.Map<IEnumerable<CourseAllDto>>(courses);
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
        [Protected]
        [HttpPut("status")]
        [Permission(PermissionSlug.COURSE_ALL)]
        public async Task<IActionResult> UpdateCourseStatus(Guid id, bool status)
        {
            var response = new ResponseDto();
            try
            {
                var courseToUpdate = await _courseRepo.GetCourseById(id);
                courseToUpdate.Status = status ? Course.CourseStatus.Active : Course.CourseStatus.Inactive;
                var result = await _courseRepo.UpdateCourse(courseToUpdate);
                if (result)
                {
                    response.Message = "Course status updated successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Course status not updated";
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
