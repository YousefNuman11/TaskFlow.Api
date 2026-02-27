using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;
using TaskFlow.Api.Data;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Services;
using System.Security.Claims;

namespace TaskFlow.Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        // GET all tasks for current user
        app.MapGet("/tasks", async(TaskService service, ClaimsPrincipal user) => {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var tasks = await service.GetAllAsync(userId);
            return Results.Ok(tasks);
        }).RequireAuthorization();

        // GET task by Id for current user
        app.MapGet("/tasks/{id:int}", async(int id, TaskService service, ClaimsPrincipal user) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var task = await service.GetByIdAsync(id, userId);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        }).RequireAuthorization();

        // POST new task for current user
        app.MapPost("/tasks", async(CreateTaskDtos dto, TaskService service, ClaimsPrincipal user) =>
        {
            // Validation
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return Results.BadRequest("Title is required");
            }
            if (dto.Title.Length > 200)
            {
                return Results.BadRequest("Title cannot exceed 200 characters");
            }

            // Get user ID from JWT
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var task = await service.CreateAsync(dto, userId);
            return Results.Created($"/tasks/{task.Id}", task);
        }).RequireAuthorization();

        // PUT update task for current user
        app.MapPut("/tasks/{id:int}", async(int id, UpdateTaskDto dto, TaskService service, ClaimsPrincipal user) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var task = await service.UpdateAsync(id, dto, userId);
            return task is null ? Results.NotFound() : Results.Ok(task);
        }).RequireAuthorization();

        // DELETE task for current user
        app.MapDelete("/tasks/{id:int}", async(int id, TaskService service, ClaimsPrincipal user) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var success = await service.DeleteAsync(id, userId);
            return success ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization();
    }
}