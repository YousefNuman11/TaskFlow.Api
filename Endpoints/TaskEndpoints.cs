using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;
using TaskFlow.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {


        //get all tasks

        app.MapGet("/tasks", async(AppDbContext db) => {
            var tasks = await db.Tasks.ToListAsync();
            return Results.Ok(tasks);
        })
        .WithName("GetTasks")
        .WithOpenApi();

        //GET task by Id

        app.MapGet("/tasks{id:int}", async(int id, AppDbContext db) =>
        {
            var task = await db.Tasks.FindAsync(id);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        })
        .WithName("GetTaskById")
        .WithOpenApi();


        //POST new task

        app.MapPost("/tasks", async(CreateTaskDtos dto, AppDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return Results.BadRequest("Title is required");
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                IsCompleted = false
            };

            db.Tasks.Add(task);
            await db.SaveChangesAsync();
            return Results.Created($"/tasks/{task.Id}", task);

        })
        .WithName("CreateTask")
        .WithOpenApi();

        //PUT update task

        app.MapPut("/tasks/{id:int}", async(int id, UpdateTaskDto dto, AppDbContext db) =>
        {
            var task = await db.Tasks.FindAsync(id);
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

            await db.SaveChangesAsync();

            return Results.Ok(task);
        })
        .WithName("UpdateTask")
        .WithOpenApi();


        //DELETE task

        app.MapDelete("/task/{id:int}", async(int id, AppDbContext db) =>
        {
            var task = await db.Tasks.FindAsync(id);
            if (task is null) return Results.NotFound();

            db.Tasks.Remove(task);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("DeleteTask")
        .WithOpenApi();



    }
}