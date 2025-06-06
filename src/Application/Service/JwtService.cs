﻿using DevKid.src.Application.Core;
using DevKid.src.Infrastructure.Cache;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DevKid.src.Application.Service
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, Guid sessionId, int roleId, int exp);
        Payload? ValidateToken(string token);
    }
    public class JwtService : IJwtService
    {
        private readonly byte[] _key;
        private readonly JwtSecurityTokenHandler _handler;
        private readonly ICacheService _cacheService;
        public JwtService(ICacheService cacheService)
        {
            var SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "8a21f416ac3c7de71de084e5190bb322456f5739eff177aeb5be84af1a70bc59";
            _key = System.Text.Encoding.ASCII.GetBytes(SecretKey);
            _handler = new JwtSecurityTokenHandler();
            _cacheService = cacheService;
        }

        public string GenerateToken(Guid userId, Guid sessionId, int roleId, int exp)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new("sessionId", sessionId.ToString()),
                    new("roleId", roleId.ToString()),
                    new("jti", Guid.NewGuid().ToString())
                ]),
                Issuer = userId.ToString(),
                Expires = DateTime.UtcNow.AddSeconds(exp),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _handler.CreateToken(tokenDescriptor);

            return _handler.WriteToken(token);
        }

        public Payload? ValidateToken(string token)
        {
            _handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var result = (JwtSecurityToken)validatedToken;

            var payload = new Payload()
            {
                UserId = Guid.Parse(result.Issuer),
                RoleId = int.Parse(result.Claims.First(x => x.Type == "roleId").Value),
                SessionId = Guid.Parse(result.Claims.First(x => x.Type == "sessionId").Value),
                Jti = Guid.Parse(result.Claims.First(x => x.Type == "jti").Value)
            };
            //check if token is already blacklisted
            var isBlacklisted = _cacheService.GetWait<bool>($"blacklist:jti:{payload.Jti}");
            if (isBlacklisted)
            {
                throw new Exception("Token is blacklisted");
            }

            return payload;
        }
    }
}
