namespace TaskFlow.Api.DTOs;

public record CreateTaskDtos
{
    public string Title { get; init; } = string.Empty;
}