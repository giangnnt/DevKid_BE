using DevKid.src.Application.Constant;
using DevKid.src.Application.Core;
using DevKid.src.Application.Dto.AuthDtos;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Dto.Session;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Cache;
using System.Security.Cryptography;

namespace DevKid.src.Application.Service
{
    public interface IAuthService
    {
        Task<ResponseDto> Login(LoginDto loginDto);
        Task<ResponseDto> Register(RegisterDto registerDto);
        Task<ResponseDto> RefreshToken(string token);
        Task<ResponseDto> Logout(string token);
    }
    public class AuthService : IAuthService
    {
        private readonly IRoleBaseRepository _roleBaseRepository;
        private readonly IUserRepo _userRepo;
        private readonly ICrypto _crypto;
        private readonly IJwtService _jwtService;
        private readonly ICacheService _cacheService;
        public AuthService(IRoleBaseRepository roleBaseRepository, IUserRepo userRepo, ICrypto crypto, IJwtService jwtService, ICacheService cacheService)
        {
            _roleBaseRepository = roleBaseRepository;
            _userRepo = userRepo;
            _crypto = crypto;
            _jwtService = jwtService;
            _cacheService = cacheService;
        }

        public async Task<ResponseDto> Login(LoginDto loginDto)
        {
            var response = new ResponseDto();
            try
            {
                // Check if user exists
                var user = await _userRepo.GetUserByEmail(loginDto.Email);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                // Verify password
                if (!_crypto.VerifyPassword(loginDto.Password, user!.Password))
                {
                    response.StatusCode = 401;
                    response.Message = "Invalid password";
                    response.IsSuccess = false;
                    return response;
                }
                Guid sessionId = Guid.NewGuid();
                var accessToken = _jwtService.GenerateToken(user.Id, sessionId, user.RoleId, JwtConst.ACCESS_TOKEN_EXP);
                var refreshToken = GenerateRefreshTk();

                // create redis refresh token key
                var redisRfTkKey = $"rfTk:{refreshToken}";
                await _cacheService.Set(redisRfTkKey, new RedisSession
                {
                    UserId = user.Id,
                    SessionId = sessionId
                }, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));

                // create a session id key
                var sessionKey = $"session:{user.Id}:{sessionId}";
                await _cacheService.Set(sessionKey, new RedisSession
                {
                    UserId = user.Id,
                    SessionId = sessionId,
                    Refresh = refreshToken
                }, TimeSpan.FromSeconds(JwtConst.REFRESH_TOKEN_EXP));
                TokenResp tokenResp = new TokenResp
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExp = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds(),
                    RefreshTokenExp = DateTimeOffset.UtcNow.AddSeconds(JwtConst.REFRESH_TOKEN_EXP).ToUnixTimeSeconds()
                };
                response.StatusCode = 200;
                response.Message = "Login successful";
                response.Result = new ResultDto
                {
                    Data = tokenResp
                };
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> RefreshToken(string token)
        {
            var response = new ResponseDto();
            try
            {
                // check if token exists
                var redisRfTkKey = $"rfTk:{token}";
                var redisrfTk = await _cacheService.Get<RedisSession>(redisRfTkKey);
                if (redisrfTk == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Token not found";
                    response.IsSuccess = false;
                    return response;
                }
                var ssId = redisrfTk.SessionId;
                var userId = redisrfTk.UserId;
                var user = await _userRepo.GetUser(redisrfTk.UserId);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                var redisSessionKey = $"session:{userId}:{ssId}";
                var redisSession = await _cacheService.Get<RedisSession>(redisSessionKey);
                if (redisSession == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Session not found";
                    response.IsSuccess = false;
                    return response;
                }
                if (redisSession.Refresh != token)
                {
                    response.StatusCode = 401;
                    response.Message = "Invalid token";
                    response.IsSuccess = false;
                    return response;
                }
                var newAccessToken = _jwtService.GenerateToken(user.Id, ssId, user.RoleId, JwtConst.ACCESS_TOKEN_EXP);
                TokenResp tokenResp = new TokenResp
                {
                    AccessToken = newAccessToken,
                    RefreshToken = token,
                    AccessTokenExp = DateTimeOffset.UtcNow.AddSeconds(JwtConst.ACCESS_TOKEN_EXP).ToUnixTimeSeconds(),
                    RefreshTokenExp = -1
                };
                response.StatusCode = 200;
                response.Message = "Token refreshed";
                response.Result = new ResultDto
                {
                    Data = tokenResp
                };
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> Register(RegisterDto registerDto)
        {
            var response = new ResponseDto();
            try
            {
                // Check if user exists
                var user = await _userRepo.GetUserByEmail(registerDto.Email);
                if (user != null)
                {
                    response.StatusCode = 400;
                    response.Message = "User already exists";
                    response.IsSuccess = false;
                }
                else
                {
                    var hashedPassword = _crypto.HashPassword(registerDto.Password);
                    var newUser = new Student
                    {
                        Email = registerDto.Email,
                        Password = hashedPassword,
                        Name = registerDto.FullName,
                        Phone = registerDto.PhoneNumber,
                        IsActive = true,
                        AvatarUrl = registerDto.AvatarUrl,
                        RoleId = 3,
                    };
                    var result = await _userRepo.AddUser(newUser);
                    if (result)
                    {
                        response.StatusCode = 201;
                        response.Message = "User created successfully";
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Message = "Failed to create user";
                        response.IsSuccess = false;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        private static string GenerateRefreshTk()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<ResponseDto> Logout(string token)
        {
            var response = new ResponseDto();
            try
            {
                var redisRfTkKey = $"rfTk:{token}";
                var redisrfTk = await _cacheService.Get<RedisSession>(redisRfTkKey);
                if (redisrfTk == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Token not found";
                    response.IsSuccess = false;
                    return response;
                }
                var ssId = redisrfTk.SessionId;
                var userId = redisrfTk.UserId;
                var user = await _userRepo.GetUser(redisrfTk.UserId);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                var redisSessionKey = $"session:{userId}:{ssId}";
                var redisSession = await _cacheService.Get<RedisSession>(redisSessionKey);
                if (redisSession == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Session not found";
                    response.IsSuccess = false;
                    return response;
                }
                if (redisSession.Refresh != token)
                {
                    response.StatusCode = 401;
                    response.Message = "Invalid token";
                    response.IsSuccess = false;
                    return response;
                }
                // remove refresh token and session
                await _cacheService.Remove(redisRfTkKey);
                await _cacheService.Remove(redisSessionKey);

                response.StatusCode = 200;
                response.Message = "Logout successful";
                response.IsSuccess = true;
                return response;

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
