using Microsoft.EntityFrameworkCore;
using DevKid.src.Infrastructure.Context;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Repository;
using DevKid.src.Application.Service;
using static DevKid.src.Application.Service.IPayOSService;
using DevKid.src.Infrastructure.Cache;
using StackExchange.Redis;
using DevKid.src.Application.Core;

var builder = WebApplication.CreateBuilder(args);
// Create a logger


var logger = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole()).CreateLogger<Program>();


// Cors Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5173", "http://localhost:5173", "https://localhost:8080", "https://127.0.0.1:8080", "https://localhost:5173", "https://127.0.0.1:5173")
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
builder.Services.AddControllers();
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
