using QualityProject.Services;
using Microsoft.EntityFrameworkCore;
using QualityProject;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using QualityProject.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "QualityProject", Version = "v1" });

    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Basic Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
Startup.Run(app);