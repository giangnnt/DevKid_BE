using Microsoft.EntityFrameworkCore;
using DevKid.src.Infrastructure.Context;
using DevKid.src.Domain.IRepository;
using DevKid.src.Infrastructure.Repository;
using DevKid.src.Application.Controller;
using DevKid.src.Domain.Entities;
using DevKid.src.Application.ExternalService;
using static DevKid.src.Application.ExternalService.IPayOSService;

var builder = WebApplication.CreateBuilder(args);
// Create a logger


var logger = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole()).CreateLogger<Program>();


// Cors Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5173", "http://localhost:5173", "https://localhost:8080", "https://127.0.0.1:8080")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});


// Services
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IPayOSService, PayOSService>();


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


// Database
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

app.MapControllers();

//app.MapMaterialEndpoints();

app.Run();
