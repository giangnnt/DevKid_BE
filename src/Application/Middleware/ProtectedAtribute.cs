using DevKid.src.Application.Dto.ResponseDtos;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using DevKid.src.Application.Service;
using DevKid.src.Infrastructure.Cache;

namespace DevKid.src.Application.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ProtectedAttribute : Attribute, IAuthorizationFilter
    {

        public ProtectedAttribute()
        {

        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var cacheService = serviceProvider.GetService<ICacheService>();
            var jwtService = new JwtService(cacheService ?? throw new Exception("Cache service not found"));
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                // Trả về sớm nếu token không có hoặc không đúng định dạng
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult(new ResponseDto
                {
                    StatusCode = 401,
                    Message = "Unauthorized",
                    IsSuccess = false
                });
                return;
            }

            var token = authorizationHeader.Replace("Bearer ", "");

            try
            {
                var payload = jwtService.ValidateToken(token);
                context.HttpContext.Items["payload"] = payload;
            }
            catch (Exception ex)
            {
                // Token không hợp lệ
                context.Result = new JsonResult(new ResponseDto
                {
                    StatusCode = 401,
                    Message = ex.Message,
                    IsSuccess = false
                });
                return;
            }
        }

    }
}
