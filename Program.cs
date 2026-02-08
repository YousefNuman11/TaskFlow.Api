using TaskFlow.Api.Models;

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

app.MapPost("/tasks", (TaskItem task) =>
{
    task.Id = tasks.Count + 1;
    tasks.Add(task);
    return Results.Created($"/tasks/{task.Id}", task);
})
.WithName("CreateTask")
.WithOpenApi();

//PUT update task

app.MapPut("/tasks/{id:int}", (int id, TaskItem updatedTask) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task is null) return Results.NotFound();

    task.Title = updatedTask.Title;
    task.IsCompleted = updatedTask.IsCompleted;

    return Results.Ok(task);
})
.WithName("UpdatedTask")
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

app.Run();
