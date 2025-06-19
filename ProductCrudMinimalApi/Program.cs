
using Microsoft.EntityFrameworkCore;
using ProductCrudMinimalApi.EndPoints;
using ProductCrudMinimalApi.Middlewares;
using ProductCrudMinimalApi.Models;
using ProductCrudMinimalApi.Models.Data;
using ProductCrudMinimalApi.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();


app.MapProductEndpoints();

app.Run();

