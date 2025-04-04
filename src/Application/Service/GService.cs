﻿using DevKid.src.Application.Constant;
using DevKid.src.Application.Dto.AuthDtos;
using DevKid.src.Application.Dto.ResponseDtos;
using DevKid.src.Application.Dto.Session;
using DevKid.src.Domain.Entities;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Cache;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;

namespace DevKid.src.Application.Service
{
    public interface IGCService
    {
        Task<string?> UploadFileAsync(Stream fileStream, string destinationFileName, string contentType);
        Task<bool> DeleteFileAsync(string fileUrl);
        string? OAuthUrlGenerate(HttpContext context);
        Task<ResponseDto> LoginGoogleCallback(string code);
    }
    public class GCService : IGCService
    {
        private string PROJECT_ID = "";
        private string FOLDER = "";
        private readonly string BucketName;
        private readonly StorageClient StorageClient;
        private readonly string FilePrefix;
        private readonly IConfiguration _config;
        private readonly string client_id;
        private readonly string client_secret;
        private readonly string redirect_uri;
        private readonly string response_type;
        private readonly string scope;
        private readonly IUserRepo _userRepo;
        private readonly IJwtService _tokenService;
        private readonly ICacheService _cacheService;
        public GCService(IConfiguration config, HttpClient httpClient, IUserRepo userRepo, ICacheService cacheService, IJwtService jwtService)
        {
            _config = config;
            PROJECT_ID = _config["GCP:PROJECT_ID"];
            FOLDER = _config["GCP:FOLDER"];

            // Initialize the Google Cloud Storage client
            string credentialsPath = "firebase.json"; // Replace with your Service Account Key path
            var credentials = GoogleCredential.FromFile(credentialsPath);
            StorageClient = StorageClient.Create(credentials);
            BucketName = PROJECT_ID + ".firebasestorage.app"; // Default Firebase Storage bucket
            FilePrefix = "https://storage.googleapis.com/" + BucketName + "/";

            // Initialize the OAuth 2.0 configuration
            client_id = _config["GCP:client_id"];
            client_secret = _config["GCP:client_secret"];
            redirect_uri = _config["GCP:oauth_callback"];
            response_type = _config["GCP:response_type"];
            scope = _config["GCP:scope"];
            _userRepo = userRepo;
            _tokenService = jwtService;
            _cacheService = cacheService;
        }
        public async Task<string?> UploadFileAsync(Stream fileStream, string destinationFileName, string contentType)
        {
            try
            {
                var objectName = FOLDER + destinationFileName;

                // Upload the file to Firebase Storage
                var options = new UploadObjectOptions
                {
                    PredefinedAcl = PredefinedObjectAcl.PublicRead // Set the PredefinedAcl to PublicRead
                };
                await StorageClient.UploadObjectAsync(BucketName, objectName, contentType, fileStream, options);

                // Generate a URL to the uploaded content
                var fileUrl = FilePrefix + objectName;
                return fileUrl;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the upload
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                // Get the file name from the file URL
                var fileName = fileUrl.Replace(FilePrefix, "");

                // Delete the file from Firebase Storage
                await StorageClient.DeleteObjectAsync(BucketName, fileName);

                return true;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the delete
                Console.WriteLine($"Error deleting file: {ex.Message}");
                return false;
            }
        }

        public string? OAuthUrlGenerate(HttpContext context)
        {
            string state = Guid.NewGuid().ToString();
            context.Session.SetString("OAuthState", state);
            var uriBuilder = new UriBuilder("https://accounts.google.com/o/oauth2/v2/auth")
            {
                Query = $"client_id={client_id}&redirect_uri={redirect_uri}&response_type={response_type}&scope={scope}&state={state}"
            };
            return uriBuilder.Uri.ToString();
        }

        public async Task<ResponseDto> LoginGoogleCallback(string code)
        {
            var response = new ResponseDto();
            HttpClient httpClient = new HttpClient();
            var uriBuilder = new UriBuilder("https://oauth2.googleapis.com/token")
            {
                Query = $"client_id={client_id}&client_secret={client_secret}&code={code}&redirect_uri={redirect_uri}&grant_type=authorization_code"
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri);
            var googleResponse = await httpClient.SendAsync(requestMessage);
            if (googleResponse.IsSuccessStatusCode)
            {
                var jsonResponse = await googleResponse.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GoogleTokenResp>(jsonResponse);
                if (result == null)
                {
                    throw new InvalidOperationException("Deserialization returned null.");
                }
                // get user info
                var userInfo = await UserInfo(result.AccessToken);
                // check if user is registered
                var user = await _userRepo.GetUserByEmail(userInfo.Email);
                if (user == null)
                {
                    // register user
                    user = await RegisterUser(userInfo);
                }
                // generate token
                Guid sessionId = Guid.NewGuid();
                var accessToken = _tokenService.GenerateToken(user.Id, sessionId, user.RoleId, JwtConst.ACCESS_TOKEN_EXP);
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

                    Data = new
                    {
                        tokenResp,
                        RedirectUri = "/",
                    }
                };
                return response;
            }
            else
            {
                var errorContent = await googleResponse.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Error when calling Google API: {errorContent}");
            }
        }
        public async Task<GoogleUserInfo> UserInfo(string accessToken)
        {
            HttpClient httpClient = new HttpClient();
            // add the access token to the request header
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var uriUserInfo = new UriBuilder("https://www.googleapis.com/oauth2/v2/userinfo");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uriUserInfo.Uri);
            // send the request
            var response = await httpClient.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GoogleUserInfo>(jsonResponse);
                if (result == null)
                {
                    throw new InvalidOperationException("Deserialization returned null.");
                }
                return result;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Error when calling Google API: {errorContent}");
            }
        }
        public async Task<User> RegisterUser(GoogleUserInfo userInfo)
        {
            var user = new Student
            {
                Id = Guid.NewGuid(),
                Email = userInfo.Email,
                Name = userInfo.Name,
                Password = "",
                RoleId = 3,
                IsActive = true,
                Phone = "0000000000",
            };
            await _userRepo.AddUser(user);
            var userTemp = await _userRepo.GetUserByEmail(userInfo.Email);
            if (userTemp == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return userTemp;
        }
        private static string GenerateRefreshTk()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
