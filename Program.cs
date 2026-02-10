using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;

var builder = WebApplication.CreateBuilder(args);


var tasks = new List<TaskItem>();

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//get all tasks

app.MapGet("/tasks", () => tasks)
    .WithName("GetTasks")
    .WithOpenApi();

//GET task by Id

app.MapGet("/tasks{id:int}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    return task is not null ? Results.Ok(task) : Results.NotFound();
})
.WithName("GetTaskById")
.WithOpenApi();

//POST new task

app.MapPost("/tasks", (CreateTaskDtos dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Title))
    {
        return Results.BadRequest("Title is required");
    }

    var task = new TaskItem
    {
        Id = tasks.Count + 1,
        Title = dto.Title,
        IsCompleted = false
    };

    tasks.Add(task);
    return Results.Created($"/tasks/{task.Id}", task);

})
.WithName("CreateTask")
.WithOpenApi();


//PUT update task

app.MapPut("/tasks/{id:int}", (int id, UpdateTaskDto dto) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null)
    {
        return Results.NotFound();
    }

    if (string.IsNullOrWhiteSpace(dto.Title))
    {
        return Results.BadRequest("Title is requird");
    }

    task.Title = dto.Title;
    task.IsCompleted = dto.IsCompleted;

    return Results.Ok(task);
})
.WithName("UpdateTask")
.WithOpenApi();


//DELETE task

app.MapDelete("/task/{id:int}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    tasks.Remove(task);
    return Results.NoContent();
})
.WithName("DeleteTask")
.WithOpenApi();




// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "OK",
        service = "TaskFlow API",
        time = DateTime.UtcNow
    });
})
.WithName("HealthCheck")
.WithOpenApi();


app.Run();
