using System.Text;
using API.Application.Interfaces;
using API.Application.Services;
using API.Domain.Services;
using API.Domain.Repositories;
using API.Infrastructure.Persistence;
using API.Infrastructure.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); 

builder.Configuration["ConnectionStrings:DefaultConnection"] = Env.GetString("MYSQL_CONNECTION_STRING");
builder.Configuration["ConnectionStrings:Redis"] = Env.GetString("REDIS_CONNECTION_STRING");
builder.Configuration["Jwt:Key"] = Env.GetString("JWT_SECRET");
builder.Configuration["Jwt:Issue"] = Env.GetString("JWT_ISSUER");
builder.Configuration["Jwt:Audience"] = Env.GetString("JWT_AUDIENCE");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });
});

builder.Services.AddSingleton<IHealthCheckPublisher, HealthCheckLogger>();
builder.Services.AddHostedService<HealthCheckBackgroundService>();
builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
    options.Delay = TimeSpan.FromSeconds(2);
    options.Period = TimeSpan.FromSeconds(60);
});

// Ver o uso dessa brincadeira aqui
// builder.Services.AddMvcCore()
//     .AddApiExplorer()
//     .AddAuthorization(options =>
//     {
//         options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
//         options.AddPolicy("User", policy => policy.RequireRole("User"));
//     });

// Add services and interfaces to the container.
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<ICacheRepository, CacheRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<ISystemService, SystemService>();
builder.Services.AddTransient<IPasswordService, PasswordService>();
builder.Services.AddTransient<ILocalesService, LocalesService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<INotificationContext, NotificationContext>();

//AutoMapper configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();

// Configure database context with MySQL
var connectionString = builder.Configuration["MYSQL_CONNECTION_STRING"];
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddHealthChecks().AddDbContextCheck<DatabaseContext>("Database");
    
// Configure Redis context
var redisConnection = builder.Configuration.GetConnectionString("Redis");
if (string.IsNullOrEmpty(redisConnection))
{
    throw new InvalidOperationException("Redis connection string is missing.");
}
builder.Services.AddSingleton(sp => new RedisContext(redisConnection));
builder.Services.AddHealthChecks()
    .AddRedis(redisConnection, name: "Redis", failureStatus:
         Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);

// Configure JWT authentication
var jwtSecret = builder.Configuration["JWT_SECRET"];
if (string.IsNullOrEmpty(jwtSecret))
    throw new InvalidOperationException("JWT secret is missing.");

var key = Encoding.ASCII.GetBytes(jwtSecret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Em produção, deve ser true
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (string.IsNullOrEmpty(context.Token) && 
                context.Request.Cookies.ContainsKey("access_token"))
            {
                context.Token = context.Request.Cookies["access_token"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCors", builder =>
        builder.WithOrigins("http://localhost:5173")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader());

    options.AddPolicy("ProductionCors", builder =>
        builder.WithOrigins("https://your-production-url.com")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        if (exceptionHandlerPathFeature?.Error != null)
        {
            logger.LogError(exceptionHandlerPathFeature.Error,
                "Error path {Path}", exceptionHandlerPathFeature.Path);
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            status = 500,
            message = "Occurred an error. Try later, please.",
            path = exceptionHandlerPathFeature?.Path
        });
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors("DevelopmentCors");
    app.MapOpenApi();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
} else 
{
    app.UseHsts();
    app.UseCors("ProductionCors");
}

app.Urls.Add("http://*:5050");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
