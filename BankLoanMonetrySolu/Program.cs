using AutoMapper;
using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.DTO;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Logic;
using BankLoanMonetrySolu.Model;
using BankLoanMonetrySolu.Services;
using Contracts.Common;
using Contracts.Dto;
using Contracts.Events;
using Contracts.Notifications;
using Contracts.Publishing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
// Added NLog namespaces
using NLog;
using NLog.Web;

// 1. Initialize NLog early to catch startup errors
var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

try
{
    logger.Info("Application is starting up..."); // Use Info to ensure it meets the minlevel in your config

    var builder = WebApplication.CreateBuilder(args);

    // 2. Clear default providers and use NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // --------------------- Services ---------------------

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            options.JsonSerializerOptions.MaxDepth = 64;
        });

    // Database
    builder.Services.AddDbContext<DALContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DbConn")));

    // Application Services
    builder.Services.AddScoped<ReadExcelFile>();
    builder.Services.AddScoped<ICAT, CAT>();
    builder.Services.AddScoped<IFinacleData, FinacleDataService>();
    builder.Services.AddScoped<ITAT, TATService>();
    builder.Services.AddScoped<IAccount, AccountService>();
    builder.Services.AddScoped<INotificationService, NotificationService>();
    builder.Services.AddScoped<IEmailServices, EmailService>();
    builder.Services.AddScoped<IAuditLookupService, AuditLookupService>();
    builder.Services.AddScoped<IConventTracker, ConventTracker>();
    builder.Services.AddHttpContextAccessor();

    // Identity
    builder.Services.AddIdentity<AppUsers, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<DALContext>()
    .AddDefaultTokenProviders();

    // --------------------- JWT Authentication ---------------------

    builder.Services.Configure<JwtSettings>(
        builder.Configuration.GetSection("JwtSettings"));

    var jwtSettings = builder.Configuration
        .GetSection("JwtSettings")
        .Get<JwtSettings>();

    if (jwtSettings == null || string.IsNullOrWhiteSpace(jwtSettings.Key))
    {
        // NLog will catch this critical startup error
        throw new Exception("JWT Key is missing in configuration");
    }

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "Bearer";
            options.DefaultChallengeScheme = "Bearer";
        })
        .AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });

    builder.Services.AddAuthorization();

    // --------------------- AutoMapper ---------------------
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // --------------------- Event Handlers ---------------------
    builder.Services.AddScoped<IEventPublisher, EventPublisher>();
    builder.Services.AddScoped<IEventHandler<ApprovedEvents>, ApprovedEventHandler>();
    builder.Services.AddScoped<IEventHandler<ConfirmedEvent>, ConfirmedEventHandler>();
    builder.Services.AddScoped<IEventHandler<RejectedEvent>, RejectedEventHandler>();
    builder.Services.AddScoped<IEventHandler<OnbordEvent>, OnboardEventHandler>();
    builder.Services.AddScoped<IEventHandler<TATEvent>, TATEventHandler>();

    builder.Services.AddHostedService<TATMonitorService>();

    // --------------------- Swagger ---------------------
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "BankLoan API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http, // This tells Swagger to handle the 'Bearer' prefix automatically
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "Just paste your JWT token below (Do NOT type 'Bearer' manually)"
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
                Array.Empty<string>()
            }
        });
    });

    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("StaffPortalPolicy", policy =>
        {
            policy.WithOrigins(allowedOrigins) 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
    });

    var app = builder.Build();

    // --------------------- Middleware ---------------------

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "BankLoan API V1");
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication(); 
    app.UseCors("StaffPortalPolicy");
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // 3. Log errors that stop the program from starting
    logger.Error(exception, "Stopped program because of exception during startup");
    throw;
}
finally
{
    // 4. Ensure logs are flushed before closing
    LogManager.Shutdown();
}