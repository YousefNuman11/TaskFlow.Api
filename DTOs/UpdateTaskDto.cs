namespace TaskFlow.Api.DTOs;

public record UpdateTaskDto
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public string Status { get; set; } = "Pending";
}