using MedOps.Infrastructure.Data;
using MedOps.Infrastructure.Services;
using MedOps.Infrastructure.Interfaces;
using MedOps.Application.Interfaces;
using MedOps.Application.Services;
using MedOps.Domain.Entities;
using MedOps.Domain.Interfaces;
using MedOps.Api.Middleware;
using MedOps.Api.Services;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MedOps Admin API",
        Version = "v1",
        Description = "MedOps Admin Platform API"
    });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<MedOpsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<MedOpsDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped(typeof(IRepository<>), typeof(MedOps.Infrastructure.Repositories.Repository<>));

builder.Services.AddScoped<IStudyService, StudyService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAuditService, MedOps.Infrastructure.Services.AuditService>();
builder.Services.AddScoped<INotificationService, MedOps.Infrastructure.Services.NotificationService>();
builder.Services.AddScoped<ICommentService, MedOps.Infrastructure.Services.CommentService>();
builder.Services.AddScoped<IFileService, MedOps.Infrastructure.Services.FileService>();
builder.Services.AddScoped<IDashboardService, MedOps.Infrastructure.Services.DashboardService>();
builder.Services.AddScoped<MedOps.Infrastructure.Services.IActivityLogService, MedOps.Infrastructure.Services.ActivityLogService>();

builder.Services.AddScoped<MedOps.Application.Validators.CreateStudyValidator>();
builder.Services.AddScoped<MedOps.Application.Validators.UpdateStudyValidator>();
builder.Services.AddScoped<MedOps.Application.Validators.CreateSiteValidator>();
builder.Services.AddScoped<MedOps.Application.Validators.UpdateSiteValidator>();
builder.Services.AddScoped<MedOps.Application.Validators.CreateTaskValidator>();
builder.Services.AddScoped<MedOps.Application.Validators.UpdateTaskValidator>();
builder.Services.AddScoped<MedOps.Application.Validators.CreateRequestValidator>();
builder.Services.AddScoped<MedOps.Application.Validators.UpdateRequestValidator>();
builder.Services.AddScoped<MedOps.Application.Validators.CreateDepartmentValidator>();
builder.Services.AddScoped<MedOps.Application.Validators.UpdateDepartmentValidator>();

builder.Services.AddSingleton<IAzureBlobService>(sp =>
    new AzureBlobService(sp.GetRequiredService<IConfiguration>().GetSection("Azure:Storage:ConnectionString").Value ?? string.Empty));
builder.Services.AddSingleton<IAzureTableService>(sp =>
    new AzureTableService(sp.GetRequiredService<IConfiguration>().GetSection("Azure:Tables:ConnectionString").Value ?? string.Empty, "default"));
builder.Services.AddSingleton<IRedisCacheService>(sp =>
    new RedisCacheService(sp.GetRequiredService<IConfiguration>().GetSection("Redis:ConnectionString").Value ?? string.Empty));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;
});

builder.Services.AddGraphQLServer()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddMutationType();

builder.Services.AddSignalR();
builder.Services.AddScoped<MedOps.Api.Services.INotificationHubService, MedOps.Api.Services.NotificationHubService>();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddDirectoryBrowser();

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetSection("Redis:ConnectionString").Value ?? string.Empty, name: "redis");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MedOpsDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MedOps Admin API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.MapHub<MedOps.Api.Hubs.NotificationHub>("/hubs/notifications");

app.MapGraphQL();

app.MapHealthChecks("/health");

app.Run();