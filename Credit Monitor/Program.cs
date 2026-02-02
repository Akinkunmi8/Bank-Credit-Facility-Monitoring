using System.Text.Json.Serialization;
using Credit_Monitor.APICore;
using Credit_Monitor.Interfaces;
using Credit_Monitor.Repository;
using Credit_Monitor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ILoanService, LoanService>();
    builder.Services.AddScoped<ILoanRepository, LoanRepository>();
    builder.Services.AddScoped<IBackendAPIService, BackendAPIService>();
    builder.Services.AddScoped<IDashboardAPI, DashboardAPI>();
    builder.Services.AddScoped<ILoginAndRegisterAPIService, LoginAndRegisterAPIService>();
    builder.Services.AddScoped<ILoanAPIService, LoanAPIService>();
    builder.Services.AddScoped<ICatAPIService, CatAPIService>();
    builder.Services.AddScoped<IFileAPIService, FileAPIService>();
    builder.Services.AddScoped<IFileService, FileService>();
    builder.Services.AddScoped<ICovenantTrackerAPIService, CovenantTrackerAPIService>();
    builder.Services.AddScoped<ICovenantTrackerService, CovenantTrackerService>();

    builder.Services.AddHttpClient<IBackendAPIService, BackendAPIService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["BackendAPI:BaseUrl"]);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    });

    builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    {
        options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.SerializerOptions.PropertyNameCaseInsensitive = true;
    });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Home/Login";
            options.AccessDeniedPath = "/Home/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromHours(1);
        });

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseSession();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception during startup");
    throw;
}
finally
{
    LogManager.Shutdown();
}