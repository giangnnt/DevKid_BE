﻿using DevKid.src.Application.Core;
using DevKid.src.Application.Service;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Cache;
using DevKid.src.Infrastructure.Context;
using DevKid.src.Infrastructure.Repository;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.Features;
using static DevKid.src.Application.Service.IPayOSService;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
// Create a logger


var logger = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole()).CreateLogger<Program>();


// Cors Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5173", "http://localhost:5173", "https://localhost:8080", "https://127.0.0.1:8080", "https://localhost:5173", "https://127.0.0.1:5173", "https://devkid.vercel.app")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

// Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Services
builder.Services.AddControllers().AddOData(options =>
{
    options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IPayOSService, PayOSService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICrypto, Crypto>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGCService, GCService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IBoughtCertificateService, BoughtCertificateService>();


// Repositories
builder.Services.AddScoped<ICourseRepo, CourseRepo>();
builder.Services.AddScoped<IChapterRepo, ChapterRepo>();
builder.Services.AddScoped<ILessonRepo, LessonRepo>();
builder.Services.AddScoped<IMaterialRepo, MaterialRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IFeedbackRepo, FeedbackRepo>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();
builder.Services.AddScoped<IMaterialRepo, MaterialRepo>();
builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IRoleBaseRepository, RoleBaseRepository>();
builder.Services.AddScoped<IStudentQuizRepo, StudentQuizRepo>();
builder.Services.AddScoped<IQuizRepo, QuizRepo>();

// redis configuration
Console.WriteLine($"Redis connection string: {builder.Configuration.GetConnectionString("RedisConnection")}");
var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection") ?? throw new InvalidOperationException("Redis connection string is not configured.");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

// SQL config
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddDbContext<DevKidContext>(options =>
{
    options.UseSqlServer(connectionString);
});


// Log configuration sources and values
logger.LogInformation("Logging configuration sources and values:");
var configurationRoot = (IConfigurationRoot)builder.Configuration;
foreach (var provider in configurationRoot.Providers)
{
    if (provider.TryGet("ConnectionStrings:DatabaseConnection", out var value))
    {
        logger.LogInformation("Provider: {Provider}, ConnectionStrings:DatabaseConnection: {Value}", provider, value);
    }
}


// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DevKid API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
          new OpenApiSecurityScheme
          {
              Reference = new OpenApiReference
              {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
              }
          },
          new string[] {}
      }
    });
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 500000000;
});

builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
logger.LogInformation("Current Environment: {Environment}", app.Environment.EnvironmentName);


app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevKid API V1");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigins");

app.UseAuthorization();

app.UseSession();

app.MapControllers();

//app.MapMaterialEndpoints();

app.Run();
