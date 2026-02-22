using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskFlow.Api.Data;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services;

public class TaskService
{
    private readonly AppDbContext _db;
    private readonly ILogger<TaskService> _logger;

    public TaskService(AppDbContext db, ILogger<TaskService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {

        _logger.LogInformation("Fetching all tasks from database.");

        return await _db.Tasks.ToListAsync();

    }


    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching task with ID {TaskId}", id);

        var task = await _db.Tasks.FindAsync(id);

        if (task is null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found.", id);
        }

        return task;
    }

    public async Task<TaskItem> CreateAsync(CreateTaskDtos dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            IsCompleted = false
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Task created with ID {TaskId}", task.Id);


        return task;

    }


    public async Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null)
        {
            _logger.LogWarning("Update failed. Task with ID {TaskId} not found.", id);
            return null;

        }

        task.Title = dto.Title;
        task.IsCompleted = dto.IsCompleted;

        await _db.SaveChangesAsync();

        _logger.LogInformation("Task with ID {TaskId} updated.", id);

        return task;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null)
        {
            _logger.LogWarning("Delete failed. Task with ID {TaskId} not found.", id);
            return false;
        }

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Task with ID {TaskId} deleted.", id);

        return true;
    }
}

