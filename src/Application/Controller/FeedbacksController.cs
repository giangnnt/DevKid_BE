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
    [Route("api/feedbacks")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackRepo _feedbackRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;
        private readonly ICourseRepo _courseRepo;
        private readonly IOrderRepo _orderRepo;

        public FeedbacksController(IFeedbackRepo feedbackRepo, IMapper mapper, IUserRepo userRepo, ICourseRepo courseRepo, IOrderRepo orderRepo)
        {
            _feedbackRepo = feedbackRepo;
            _mapper = mapper;
            _userRepo = userRepo;
            _courseRepo = courseRepo;
            _orderRepo = orderRepo;
        }

        //// GET: api/Feedbacks
        //[HttpGet]
        //public async Task<ActionResult> GetFeedbacks()
        //{
        //    var response = new ResponseDto();
        //    try
        //    {
        //        var feedbacks = await _feedbackRepo.GetFeedbacks();
        //        if (feedbacks != null)
        //        {
        //            response.Message = "Feedbacks fetched successfully";
        //            response.Result = new ResultDto
        //            {
        //                Data = _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks)
        //            };
        //            response.IsSuccess = true;
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            response.Message = "Feedbacks not fetched";
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

        // GET: api/Feedbacks/5
        [HttpGet]
        public async Task<ActionResult> GetFeedbacksByCourseId(Guid courseId)
        {
            var response = new ResponseDto();
            try
            {
                var feedbacks = await _feedbackRepo.GetFeedbacksByCourseId(courseId);
                if (feedbacks != null)
                {
                    response.Message = "Feedbacks fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Feedbacks not fetched";
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
        public async Task<ActionResult> GetFeedback(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var feedback = await _feedbackRepo.GetFeedback(id);
                if (feedback != null)
                {
                    response.Message = "Feedback fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<FeedbackDto>(feedback)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Feedback not fetched";
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
        [Permission(PermissionSlug.FEEDBACK_ALL, PermissionSlug.FEEDBACK_OWN)]
        public async Task<IActionResult> PutFeedback(Guid id, FeedbackUpdateDto feedback)
        {
            var response = new ResponseDto();
            try
            {
                var mappedFeedback = _mapper.Map<Feedback>(feedback);
                var result = await _feedbackRepo.UpdateFeedback(mappedFeedback);
                if (result)
                {
                    response.Message = "Feedback updated successfully";
                    response.IsSuccess = true;
                    return Created("", response);
                }
                else
                {
                    response.Message = "Feedback not updated";
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
        [Permission(PermissionSlug.FEEDBACK_ALL, PermissionSlug.FEEDBACK_OWN)]
        public async Task<ActionResult<Feedback>> PostFeedback(FeedbackCreateDto feedback)
        {

            var response = new ResponseDto();
            try
            {
                var payload = HttpContext.Items["payload"] as Payload;
                // check if payload is null
                if (payload == null)
                {
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return Unauthorized(response);
                }
                var course = await _courseRepo.GetCourseById(feedback.CourseId);
                var student = await _userRepo.GetUser(feedback.StudentId);
                var mappedFeedback = _mapper.Map<Feedback>(feedback);
                // check if student bought the course
                if (!await _orderRepo.HaveUserBoughtCourse(course.Id, student.Id))
                {
                    response.Message = "You have not bought this course";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
                // check if student already gave feedback
                if (await _feedbackRepo.HaveUserFeedbackOnCourse(course.Id, student.Id))
                {
                    response.Message = "You have already given feedback";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
                var result = await _feedbackRepo.AddFeedback(mappedFeedback);
                if (result)
                {
                    response.Message = "Feedback added successfully";
                    response.IsSuccess = true;
                    return Created("", response);
                }
                else
                {
                    response.Message = "Feedback not added";
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
        [Permission(PermissionSlug.FEEDBACK_ALL, PermissionSlug.FEEDBACK_OWN)]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _feedbackRepo.DeleteFeedback(id);
                if (result)
                {
                    response.Message = "Feedback deleted successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Feedback not deleted";
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
