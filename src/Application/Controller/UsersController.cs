using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevKid.src.Domain.Entities;
using DevKid.src.Infrastructure.Context;
using DevKid.src.Infrastructure.Repository;
using DevKid.src.Domain.IRepository;
using AutoMapper;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Dto;
using DevKid.src.Application.Middleware;
using DevKid.src.Application.Constant;
using DevKid.src.Application.Core;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepo userRepo, IMapper mapper)
        {
            _mapper = mapper;
            _userRepo = userRepo;
        }

        [Protected]
        [HttpGet]
        [Permission(PermissionSlug.USER_ALL, PermissionSlug.USER_VIEW)]
        public async Task<ActionResult> GetUsers()
        {
            var response = new ResponseDto();
            try
            {
                var users = await _userRepo.GetUsers();
                if (users != null)
                {
                    response.Message = "Users fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<IEnumerable<UserDto>>(users)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Users not fetched";
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
        [HttpGet("{id}")]
        [Permission(PermissionSlug.USER_ALL, PermissionSlug.USER_VIEW)]
        public async Task<ActionResult> GetUser(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var user = await _userRepo.GetUser(id);
                if (user != null)
                {
                    response.Message = "User fetched successfully";
                    response.Result = new ResultDto
                    {
                        Data = _mapper.Map<UserDto>(user)
                    };
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "User not fetched";
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
        [Permission(PermissionSlug.USER_ALL, PermissionSlug.USER_OWN)]
        public async Task<IActionResult> PutUser(Guid id, UserUpdateDto user)
        {
            var response = new ResponseDto();
            try
            {
                var payload = HttpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return NotFound(response);
                }
                if (payload.RoleId != RoleConst.ADMIN_ID && payload.RoleId != RoleConst.MANAGER_ID)
                {
                    if (payload.UserId != id)
                    {
                        response.Message = "You are not authorized to update this user";
                        response.IsSuccess = false;
                        return Unauthorized(response);
                    }
                }
                var userToUpdate = await _userRepo.GetUser(id);
                var mappedUser = _mapper.Map(user, userToUpdate);
                var result = await _userRepo.UpdateUser(mappedUser);
                if (result)
                {
                    response.Message = "User updated successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "User not updated";
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
        [Permission(PermissionSlug.USER_ALL)]
        public async Task<ActionResult<User>> PostUser(UserCreateDto user)
        {
            var response = new ResponseDto();
            try
            {
                var mappedUser = _mapper.Map<User>(user);
                var result = await _userRepo.AddUser(mappedUser);
                if (result)
                {
                    response.Message = "User added successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "User not added";
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
        [Permission(PermissionSlug.USER_ALL)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _userRepo.DeleteUser(id);
                if (result)
                {
                    response.Message = "User deleted successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "User not deleted";
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
        [Permission(PermissionSlug.USER_ALL)]
        public async Task<IActionResult> PutUserStatus(Guid id, bool userStatus)
        {
            var response = new ResponseDto();
            try
            {
                var userToUpdate = await _userRepo.GetUser(id);
                userToUpdate.IsActive = userStatus;
                var result = await _userRepo.UpdateUser(userToUpdate);
                if (result)
                {
                    response.Message = "User status updated successfully";
                    response.IsSuccess = true;
                    return Ok(response);
                }
                else
                {
                    response.Message = "User status not updated";
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
