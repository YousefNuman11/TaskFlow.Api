using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);


// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapTaskEndpoints();



app.Run();
