using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using DevKid.src.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);
// Create a logger
var logger = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole()).CreateLogger<Program>();
// Cors Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5173", "http://localhost:5173", "https://localhost:8080", "https://127.0.0.1:8080", "http://localhost:5000", "http://127.0.0.1:5000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});
// Services
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(Program));
// Repositories
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
var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevKid API V1");
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
