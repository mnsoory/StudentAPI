using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using StudentAPI.API.Extensions.Middlewares;
using StudentAPI.API.Filters;
using StudentAPI.API.Mappings;
using StudentAPI.Application.Behaviors;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Application.Features.Students.Handlers;
using StudentAPI.Application.Interfaces;
using StudentAPI.Application.Mappings;
using StudentAPI.Application.Validation;
using StudentAPI.Domain.Interfaces;
using StudentAPI.Infrastructure.Models;
using StudentAPI.Infrastructure.Persistence;
using StudentAPI.Infrastructure.Repositories;
using StudentAPI.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StudentAPIDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Repositories and Services DI
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(StudentProfile).Assembly);
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

// MediatR and Validation Pipeline
var applicationAssembly = typeof(GetStudentsQueryHandler).Assembly;
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(applicationAssembly);
});
builder.Services.AddValidatorsFromAssembly(applicationAssembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Controllers and Global Filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<PermissionBasedAuthorizatioFilter>();
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication Configuration
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsJsonAsync(new
                {
                    Message = "Authentication failed. The token is invalid or expired."
                });
            }
        };
    });

var app = builder.Build();

// HTTP Request Pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Database Migration on Startup
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<StudentAPIDbContext>();
        dbContext.Database.Migrate();
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating the database.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Auth Middlewares
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();