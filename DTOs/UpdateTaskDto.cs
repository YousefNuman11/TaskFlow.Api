namespace TaskFlow.Api.DTOs;

public record UpdateTaskDto
{
    public string Title { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }
}