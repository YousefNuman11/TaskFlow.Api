using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;
using TaskFlow.Api.Data;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {


        //get all tasks

        app.MapGet("/tasks", async(TaskService service) => {
            var tasks = await service.GetAllAsync();
            return Results.Ok(tasks);
        }).RequireAuthorization();

        //GET task by Id

        app.MapGet("/tasks/{id:int}", async(int id, TaskService service) =>
        {
            var task = await service.GetByIdAsync(id);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        }).RequireAuthorization();


        //POST new task

        app.MapPost("/tasks", async(CreateTaskDtos dto, TaskService service) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return Results.BadRequest("Title is required");
            }

            if (dto.Title.Length > 200)
            {
                return Results.BadRequest("Title cannot exceed 200 characters");
            }

            var task = await service.CreateAsync(dto);
            return Results.Created($"/tasks/{task.Id}", task);

        }).RequireAuthorization();

        //PUT update task

        app.MapPut("/tasks/{id:int}", async(int id, UpdateTaskDto dto, TaskService service) =>
        {
            var task = await service.UpdateAsync(id, dto);
            return task is null ? Results.NotFound() : Results.Ok(task);
        }).RequireAuthorization();


        //DELETE task

        app.MapDelete("/tasks/{id:int}", async(int id, TaskService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization();
    }
}