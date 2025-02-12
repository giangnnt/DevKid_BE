using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevKid.src.Domain.Entities;
using DevKid.src.Infrastructure.Context;
using DevKid.src.Domain.IRepository;
using AutoMapper;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Dto;

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

        public FeedbacksController(IFeedbackRepo feedbackRepo, IMapper mapper, IUserRepo userRepo, ICourseRepo courseRepo)
        {
            _feedbackRepo = feedbackRepo;
            _mapper = mapper;
            _userRepo = userRepo;
            _courseRepo = courseRepo;
        }

        // GET: api/Feedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            var response = new ResponseDto();
            try
            {
                var feedbacks = await _feedbackRepo.GetFeedbacks();
                if (feedbacks != null)
                {
                    response.Message = "Feedbacks fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<Feedback>>(feedbacks)
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

        // GET: api/Feedbacks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(Guid id)
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
                        Data = _mapper.Map<Feedback>(feedback)
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

        // PUT: api/Feedbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        // POST: api/Feedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Feedback>> PostFeedback(FeedbackCreateDto feedback)
        {
            var response = new ResponseDto();
            try
            {
                var course = await _courseRepo.GetCourseById(feedback.CourseId);
                var student = await _userRepo.GetUser(feedback.StudentId);
                var mappedFeedback = _mapper.Map<Feedback>(feedback);
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

        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
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
