﻿using DevKid.src.Application.Core;
using DevKid.src.Application.Dto.AuthDtos;
using DevKid.src.Application.Middleware;
using DevKid.src.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace DevKid.src.Application.Controller
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGCService _gCService;
        public AuthController(IAuthService authService, IGCService gCService)
        {
            _authService = authService;
            _gCService = gCService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _authService.Login(loginDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.Register(registerDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string token)
        {
            try
            {
                var result = await _authService.RefreshToken(token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Protected]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromQuery] string token)
        {
            try
            {
                var payload = HttpContext.Items["payload"] as Payload;
                if (payload is null)
                {
                    return BadRequest("Invalid token");
                }
                var result = await _authService.Logout(token, payload);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            try
            {
                var uri = _gCService.OAuthUrlGenerate(HttpContext);
                return Redirect(uri ?? throw new Exception("uri not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("login-google-callback")]
        public async Task<IActionResult> LoginWithGoogleCallback([FromQuery] string code, [FromQuery] string state)
        {
            try
            {
                var expectedState = HttpContext.Session.GetString("OAuthState");

                if (string.IsNullOrEmpty(state) || state != expectedState)
                {
                    return BadRequest("Invalid state");
                }
                var result = await _gCService.LoginGoogleCallback(code);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
