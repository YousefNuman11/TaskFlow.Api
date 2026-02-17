using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services;

public class TaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TaskItem>> GetAllAsync() => await _db.Tasks.ToListAsync();


    public async Task<TaskItem?> GetByIdAsync(int id) => await _db.Tasks.FindAsync(id);

    public async Task<TaskItem> CreateAsync(CreateTaskDtos dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            IsCompleted = false
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        return task;
        
    }

    
    public async Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _db.Tasks.FindAsync(id);
        if(task is null) return null;

        task.Title = dto.Title;
        task.IsCompleted = dto.IsCompleted;

        await _db.SaveChangesAsync();
        return task;
    } 

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if(task is null) return false;

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();

        return true;
    }
}

