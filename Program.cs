using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;



var builder = WebApplication.CreateBuilder(args);


// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();


// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapTaskEndpoints();



app.Run();
