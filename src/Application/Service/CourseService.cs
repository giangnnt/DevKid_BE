using AutoMapper;
using DevKid.src.Application.Dto.CourseDtos;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Domain.IService;

namespace DevKid.src.Application.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepo _courseRepo;
        private readonly IMapper _mapper;
        public CourseService(ICourseRepo courseRepo, IMapper mapper)
        {
            _courseRepo = courseRepo;
            _mapper = mapper;
        }

        public async Task<ResponseDto> AddCourse(CourseCreateDto course)
        {
            var response = new ResponseDto();
            try
            {
                var mappedCourse = _mapper.Map<Course>(course);
                var result = await _courseRepo.AddCourse(mappedCourse);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Course added successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Course not added";
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

        public async Task<ResponseDto> DeleteCourse(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _courseRepo.DeleteCourse(id);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Course deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Course not deleted";
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

        public async Task<ResponseDto> GetAllCourses()
        {
            var response = new ResponseDto();
            try
            {
                var courses = await _courseRepo.GetAllCourses();
                var mappedCourses = _mapper.Map<IEnumerable<CourseDto>>(courses);
                if (mappedCourses != null && mappedCourses.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Courses fetched successfully";
                    response.Result = new ResultDto { Data = mappedCourses };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No courses found";
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

        public async Task<ResponseDto> GetCourseById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var course = await _courseRepo.GetCourseById(id);
                var mappedCourse = _mapper.Map<CourseDto>(course);
                if (mappedCourse != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Course fetched successfully";
                    response.Result = new ResultDto { Data = mappedCourse };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Course not found";
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

        public async Task<ResponseDto> UpdateCourse(Guid id, CourseUpdateDto course)
        {
            var response = new ResponseDto();
            try
            {
                var courseToUpdate = await _courseRepo.GetCourseById(id);
                var mappedCourse = _mapper.Map(course, courseToUpdate);
                var result = await _courseRepo.UpdateCourse(mappedCourse);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Course updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Course not updated";
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
