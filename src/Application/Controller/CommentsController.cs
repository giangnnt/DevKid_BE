﻿using AutoMapper;
using DevKid.src.Application.Constant;
using DevKid.src.Application.Dto;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Middleware;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;
        private readonly ILessonRepo _lessonRepo;

        public CommentsController(ICommentRepo commentRepo, IMapper mapper, IUserRepo userRepo, ILessonRepo lessonRepo)
        {
            _commentRepo = commentRepo;
            _mapper = mapper;
            _userRepo = userRepo;
            _lessonRepo = lessonRepo;
        }
        //[Protected]
        //// GET: api/Comments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        //{
        //    var response = new ResponseDto();
        //    try
        //    {
        //        var comments = await _commentRepo.GetComments();
        //        if (comments != null)
        //        {
        //            response.Message = "Comments fetched successfully";
        //            response.Result = new ResultDto
        //            {
        //                Data = _mapper.Map<IEnumerable<CommentDto>>(comments)
        //            };
        //            response.IsSuccess = true;
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            response.Message = "Comments not fetched";
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
        [Permission(PermissionSlug.COMMENT_ALL, PermissionSlug.COMMNET_VIEW)]
        public async Task<ActionResult<Comment>> GetComment(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var comment = await _commentRepo.GetComment(id);
                if (comment != null)
                {
                    response.Message = "Comment fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<CommentDto>(comment)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Comment not fetched";
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
        [Permission(PermissionSlug.COMMENT_ALL, PermissionSlug.COMMENT_OWN)]
        public async Task<IActionResult> PutComment(Guid id, CommentUpdateDto comment)
        {
            var response = new ResponseDto();
            try
            {
                var commentToUpdate = await _commentRepo.GetComment(id);
                var mappedComment = _mapper.Map<Comment>(comment);
                var result = await _commentRepo.UpdateComment(mappedComment);
                if (result)
                {
                    response.Message = "Comment updated successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Comment not updated";
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
        [Permission(PermissionSlug.COMMENT_ALL, PermissionSlug.COMMENT_OWN)]
        public async Task<ActionResult<Comment>> PostComment(CommentCreateDto comment)
        {
            var response = new ResponseDto();
            try
            {
                var student = await _userRepo.GetUser(comment.StudentId);
                var lesson = await _lessonRepo.GetLessonById(comment.LessonId);
                var mappedComment = _mapper.Map<Comment>(comment);
                var result = await _commentRepo.AddComment(mappedComment);
                if (result)
                {
                    response.Message = "Comment added successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Comment not added";
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
        [Permission(PermissionSlug.COMMENT_ALL, PermissionSlug.COMMENT_OWN)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _commentRepo.DeleteComment(id);
                if (result)
                {
                    response.Message = "Comment deleted successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Comment not deleted";
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
        [HttpGet("lesson/{lessonId}")]
        [Permission(PermissionSlug.COMMENT_ALL, PermissionSlug.COMMNET_VIEW)]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByLessonId(Guid lessonId)
        {
            var response = new ResponseDto();
            try
            {
                var comments = await _commentRepo.GetCommentsByLessonId(lessonId);
                if (comments != null)
                {
                    response.Message = "Comments fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<CommentDto>>(comments)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Comments not fetched";
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
