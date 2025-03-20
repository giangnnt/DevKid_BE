using AutoMapper;
using DevKid.src.Application.Constant;
using DevKid.src.Application.Core;
using DevKid.src.Application.Dto;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Middleware;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/student-quiz")]
    [ApiController]
    public class StudentQuizController : ControllerBase
    {
        private readonly IStudentQuizRepo _studentQuizRepo;
        private readonly IMapper _mapper;
        private readonly IQuizRepo _quizRepo;
        public StudentQuizController(IStudentQuizRepo studentQuizRepo, IMapper mapper, IQuizRepo quizRepo)
        {
            _studentQuizRepo = studentQuizRepo;
            _mapper = mapper;
            _quizRepo = quizRepo;
        }
        [Protected]
        [HttpGet("{id}")]
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
                var studentQuiz = await _studentQuizRepo.GetStudentQuizById(id);
                if (studentQuiz != null)
                {
                    // Check if the student is the owner of the student quiz
                    if (studentQuiz.StudentId != payload.UserId)
                    {
                        response.IsSuccess = false;
                        response.Message = "Unauthorized";
                        return BadRequest(response);
                    }
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<StudentQuizDto>(studentQuiz)
                    };
                    response.Message = "Get student quiz successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Student quiz not found";
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [Protected]
        [HttpGet("quiz/{quizId}")]
        [Permission(PermissionSlug.QUIZ_ALL, PermissionSlug.STUDENT_QUIZ_ALL)]
        public async Task<IActionResult> GetStudentQuizByQuizId(Guid quizId)
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
                var studentQuiz = await _studentQuizRepo.GetStudentQuizByStudentIdQuizId(payload.UserId, quizId);
                if (studentQuiz != null)
                {
                    if (studentQuiz.StudentId != payload.UserId)
                    {
                        response.IsSuccess = false;
                        response.Message = "Unauthorized";
                        return BadRequest(response);
                    }
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<StudentQuizDto>(studentQuiz)
                    };
                    response.Message = "Get student quiz successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Student quiz not found";
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [Protected]
        [HttpPost("submit")]
        [Permission(PermissionSlug.QUIZ_ALL, PermissionSlug.STUDENT_QUIZ_ALL)]
        public async Task<IActionResult> SubmitCreateStudentQuiz([FromBody] StudentQuizCreateDto studentQuizDto)
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
                //check any
                var studentQuizExist = await _studentQuizRepo.GetStudentQuizByStudentIdQuizId(payload.UserId, studentQuizDto.QuizId);
                if (studentQuizExist != null)
                {
                    response.IsSuccess = false;
                    response.Message = "Student quiz already exists";
                    return BadRequest(response);
                }
                // Ques Ans
                var studentQuizAns = await PreScoreCalculate(studentQuizDto.QuesAns, studentQuizDto.QuizId);
                var studentQuiz = _mapper.Map<StudentQuiz>(studentQuizDto);
                studentQuiz.StudentId = payload.UserId;
                studentQuiz.QuesAns = studentQuizAns;
                // Score
                studentQuiz.Score = (float)(studentQuizAns.Count == 0 ? 0 : (studentQuizAns.Count(x => x.Value.IsCorrect) * 10.0 / studentQuizAns.Count));
                // Status
                if (studentQuiz.Score >= 7)
                {
                    studentQuiz.Status = StudentQuiz.QuizStatus.Completed;
                }
                else
                {
                    studentQuiz.Status = StudentQuiz.QuizStatus.Uncompleted;
                }
                if (await _studentQuizRepo.CreateStudentQuiz(studentQuiz))
                {
                    response.Message = "Create student quiz successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Create student quiz failed";
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        //[Protected]
        //[HttpDelete]
        //[Permission(PermissionSlug.QUIZ_ALL)]
        //public async Task<IActionResult> DeleteStudentQuiz(Guid id)
        //{
        //    var response = new ResponseDto();
        //    try
        //    {
        //        var payload = HttpContext.Items["payload"] as Payload;
        //        if (payload == null)
        //        {
        //            response.Message = "Unauthorized";
        //            response.IsSuccess = false;
        //            return BadRequest(response);
        //        }
        //        var studentQuiz = await _studentQuizRepo.GetStudentQuizById(id);
        //        if (studentQuiz != null)
        //        {
        //            if (studentQuiz.StudentId != payload.UserId)
        //            {
        //                response.IsSuccess = false;
        //                response.Message = "Unauthorized";
        //                return BadRequest(response);
        //            }
        //            if (await _studentQuizRepo.DeleteStudentQuiz(id))
        //            {
        //                response.Message = "Delete student quiz successfully";
        //                response.IsSuccess = true;
        //                return Ok(response);
        //            }
        //            else
        //            {
        //                response.IsSuccess = false;
        //                response.Message = "Delete student quiz failed";
        //                return BadRequest(response);
        //            }
        //        }
        //        else
        //        {
        //            response.IsSuccess = false;
        //            response.Message = "Student quiz not found";
        //            return BadRequest(response);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        response.IsSuccess = false;
        //        response.Message = e.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}
        [Protected]
        [HttpPut]
        [Permission(PermissionSlug.QUIZ_ALL, PermissionSlug.STUDENT_QUIZ_ALL)]
        public async Task<IActionResult> UpdateStudentQuiz([FromBody] StudentQuizUpdateDto studentQuizDto)
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
                var studentQuiz = await _studentQuizRepo.GetStudentQuizById(studentQuizDto.QuizId);
                if (studentQuiz != null)
                {
                    if (studentQuiz.StudentId != payload.UserId)
                    {
                        response.IsSuccess = false;
                        response.Message = "Unauthorized";
                        return BadRequest(response);
                    }
                    var studentQuizAns = await PreScoreCalculate(studentQuizDto.QuesAns, studentQuizDto.QuizId);
                    studentQuiz.QuesAns = studentQuizAns;
                    studentQuiz.Score = (float)(studentQuizAns.Count == 0 ? 0 : (studentQuizAns.Count(x => x.Value.IsCorrect) * 10.0 / studentQuizAns.Count));
                    if (studentQuiz.Score >= 7)
                    {
                        studentQuiz.Status = StudentQuiz.QuizStatus.Completed;
                    }
                    else
                    {
                        studentQuiz.Status = StudentQuiz.QuizStatus.Uncompleted;
                    }
                    if (await _studentQuizRepo.UpdateStudentQuiz(studentQuiz))
                    {
                        response.Message = "Update student quiz successfully";
                        response.IsSuccess = true;
                        return Ok(response);
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Update student quiz failed";
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Student quiz not found";
                    return BadRequest(response);
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        private async Task<Dictionary<string, StudentAns>> PreScoreCalculate(Dictionary<string, List<Guid>?>? dictInstance, Guid quizId)
        {
            // check if dictInstance is null or empty, then return empty dictionary
            if (dictInstance == null || dictInstance.Count == 0 || dictInstance.Values == null || dictInstance.Values.Count == 0)
            {
                return new Dictionary<string, StudentAns>();
            }
            var studentQuizAns = new Dictionary<string, StudentAns>();
            var quizSample = await _quizRepo.GetQuizById(quizId);
            var dictSample = quizSample.QuesAns ?? new Dictionary<string, List<Ans>>();
            foreach (var dict in dictSample)
            {
                var listGuidCorrectSample = dict.Value.Where(a => a.IsCorrect == true).Select(a => a.Id).ToList();
                if(!dictInstance.ContainsKey(dict.Key))
                {
                    studentQuizAns.Add(dict.Key, new StudentAns
                    {
                        Id = new List<Guid>(),
                        IsCorrect = false
                    });
                    continue;
                }
                var listGuidCorrectStudent = dictInstance[dict.Key] ?? new List<Guid>();
                studentQuizAns.Add(dict.Key, new StudentAns
                {
                    Id = listGuidCorrectStudent,
                    IsCorrect = new HashSet<Guid>(listGuidCorrectSample).SetEquals(listGuidCorrectStudent)
                });
            }
            // scoring
            return studentQuizAns;
        }
    }
}
