using AutoMapper;
using DevKid.src.Application.Constant;
using DevKid.src.Application.Core;
using DevKid.src.Application.Dto;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Middleware;
using DevKid.src.Application.Service;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/quizzes")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizRepo _quizRepo;
        private readonly IMapper _mapper;
        private readonly IBoughtCertificateService _boughtCertificateService;
        public QuizzesController(IQuizRepo quizRepo, IMapper mapper, IBoughtCertificateService boughtCertificateService)
        {
            _quizRepo = quizRepo;
            _mapper = mapper;
            _boughtCertificateService = boughtCertificateService;
        }
        [Protected]
        [HttpGet("lesson/{id}/detail")]
        [Permission(PermissionSlug.QUIZ_ALL)]
        public async Task<IActionResult> GetQuizzesByLessonId(Guid id)
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
                var quizzes = await _quizRepo.GetQuizByLessonId(id);
                if (quizzes != null)
                {
                    // only admin and manager can access chapter detail without buying
                    if (payload.RoleId != RoleConst.ADMIN_ID && payload.RoleId != RoleConst.MANAGER_ID)
                    {
                        if (!await _boughtCertificateService.CheckCertificateAsync(id, payload.UserId))
                        {
                            response.Message = "Unauthorized";
                            response.IsSuccess = false;
                            return BadRequest(response);
                        }
                    }
                    response.Message = "Quizzes fetch successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<QuizAdminDto>>(quizzes)
                    };
                    return Ok(response);
                }
                else
                {
                    response.Message = "Quizzes not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Protected]
        [HttpGet("lesson/{id}/summary")]
        [Permission(PermissionSlug.QUIZ_ALL, PermissionSlug.STUDENT_QUIZ_ALL)]
        public async Task<IActionResult> GetStudentQuizzesByLessonId(Guid id)
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
                var quizzes = await _quizRepo.GetQuizByLessonId(id);
                if (quizzes != null)
                {
                    // only admin and manager can access chapter detail without buying
                    if (payload.RoleId != RoleConst.ADMIN_ID && payload.RoleId != RoleConst.MANAGER_ID)
                    {
                        if (!await _boughtCertificateService.CheckCertificateAsync(id, payload.UserId))
                        {
                            response.Message = "Unauthorized";
                            response.IsSuccess = false;
                            return BadRequest(response);
                        }
                    }
                    response.Message = "Quizzes fetch successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<QuizDto>>(quizzes)
                    };
                    return Ok(response);
                }
                else
                {
                    response.Message = "Quizzes not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Protected]
        [HttpGet("{id}/summary")]
        [Permission(PermissionSlug.QUIZ_ALL, PermissionSlug.STUDENT_QUIZ_ALL)]
        public async Task<IActionResult> GetStudentQuizById(Guid id)
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
                var quiz = await _quizRepo.GetQuizById(id);
                if (quiz != null)
                {
                    // only admin and manager can access chapter detail without buying
                    if (payload.RoleId != RoleConst.ADMIN_ID && payload.RoleId != RoleConst.MANAGER_ID)
                    {
                        if (!await _boughtCertificateService.CheckCertificateAsync(id, payload.UserId))
                        {
                            response.Message = "Unauthorized";
                            response.IsSuccess = false;
                            return BadRequest(response);
                        }
                    }
                    response.Message = "Quiz fetch successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<QuizDto>(quiz)
                    };
                    return Ok(response);
                }
                else
                {
                    response.Message = "Quiz not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Protected]
        [HttpGet("{id}/detail")]
        [Permission(PermissionSlug.QUIZ_ALL)]
        public async Task<IActionResult> GetQuizById(Guid id)
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
                var quiz = await _quizRepo.GetQuizById(id);
                if (quiz != null)
                {
                    // only admin and manager can access chapter detail without buying
                    if (payload.RoleId != RoleConst.ADMIN_ID && payload.RoleId != RoleConst.MANAGER_ID)
                    {
                        if (!await _boughtCertificateService.CheckCertificateAsync(id, payload.UserId))
                        {
                            response.Message = "Unauthorized";
                            response.IsSuccess = false;
                            return BadRequest(response);
                        }
                    }
                    response.Message = "Quiz fetch successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<QuizAdminDto>(quiz)
                    };
                    return Ok(response);
                }
                else
                {
                    response.Message = "Quiz not fetched";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Protected]
        [HttpPost]
        [Permission(PermissionSlug.QUIZ_ALL)]
        public async Task<IActionResult> CreateQuiz([FromBody] QuizCreateDto quizzesDto)
        {
            var response = new ResponseDto();
            try
            {
                var quiz = _mapper.Map<Quiz>(quizzesDto);
                var result = await _quizRepo.CreateQuiz(quiz);
                if (result)
                {
                    response.Message = "Quiz created successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Quiz not created";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Protected]
        [HttpPut("{id}")]
        [Permission(PermissionSlug.QUIZ_ALL)]
        public async Task<IActionResult> UpdateQuiz([FromRoute] Guid id, [FromBody] QuizUpdateDto quizDto)
        {
            var response = new ResponseDto();
            try
            {
                var quiz = await _quizRepo.GetQuizById(id);
                if (quiz != null)
                {
                    var quizUpdate = _mapper.Map(quizDto, quiz);
                    var result = await _quizRepo.UpdateQuiz(quizUpdate);
                    if (result)
                    {
                        response.Message = "Quiz updated successfully";
                        response.Result = new ResultDto
                        {
                            Data = quizUpdate.Id
                        };
                        return Ok(response);
                    }
                    else
                    {
                        response.Message = "Quiz not updated";
                        response.IsSuccess = false;
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.Message = "Quiz not found";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        [Protected]
        [HttpDelete("{id}")]
        [Permission(PermissionSlug.QUIZ_ALL)]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var quiz = await _quizRepo.GetQuizById(id);
                if (quiz != null)
                {
                    var result = await _quizRepo.DeleteQuiz(id);
                    if (result)
                    {
                        response.Message = "Quiz deleted successfully";
                        response.IsSuccess = true;
                        return Ok(response);
                    }
                    else
                    {
                        response.Message = "Quiz not deleted";
                        response.IsSuccess = false;
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.Message = "Quiz not found";
                    response.IsSuccess = false;
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
