using AutoMapper;
using BankLoanMonetrySolu.Context;
using BankLoanMonetrySolu.IServices;
using BankLoanMonetrySolu.Logic;
using BankLoanMonetrySolu.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
    });

builder.Services.AddDbContext<DALContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConn")));
builder.Services.AddScoped<ReadExcelFile>();
builder.Services.AddScoped<ICAT, CAT>();
builder.Services.AddScoped<IFinacleData, FinacleData>();
builder.Services.AddScoped<ITAT, TATService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
