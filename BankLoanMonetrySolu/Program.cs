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

var builder = WebApplication.CreateBuilder(args);

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
//builder.Services.AddSingleton<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<IEventHandler<ApprovedEvents>, ApprovedEventHandler>();
builder.Services.AddScoped<IEventHandler<ConfirmedEvent>, ConfirmedEventHandler>();
builder.Services.AddScoped<IEventHandler<RejectedEvent>, RejectedEventHandler>();
builder.Services.AddScoped<IEventHandler<OnbordEvent>, OnboardEventHandler>();
builder.Services.AddScoped<IEventHandler<TATEvent>, TATEventHandler>();

//builder.Services.AddEventHandlers(typeof(ApprovedEventHandler).Assembly);
//builder.Services.AddEventHandlers(typeof(ConfirmedEventHandler).Assembly);
//builder.Services.AddEventHandlers(typeof(RejectedEventHandler).Assembly);
//builder.Services.AddEventHandlers(typeof(TATEventHandler).Assembly);
//builder.Services.AddEventHandlers(typeof(OnboardEventHandler).Assembly);
builder.Services.AddHostedService<TATMonitorService>();

// --------------------- Swagger ---------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BankLoan API", Version = "v1" });

    // Add JWT Bearer Auth
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer' [space] and your token"
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
// 1. Read the Origins from appsettings.json
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

// 2. Add the CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("StaffPortalPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins) // Tags the allowed origins from config
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Essential if the Dashboard uses Cookies/Sessions
    });
});

var app = builder.Build();
// --------------------- Middleware ---------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // 2. Remove the leading '/' so it becomes relative to your current folder
        c.SwaggerEndpoint("v1/swagger.json", "BankLoan API V1");

        // Optional: if you want Swagger to load as the very first page
        // c.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection();
app.UseAuthentication(); // MUST come before Authorization
app.UseCors("StaffPortalPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
