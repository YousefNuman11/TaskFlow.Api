namespace TaskFlow.Api.DTOs;

public record CreateTaskDtos
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; set; } = "Task to do"; //Optional
    public DateTime? DueDate { get; set; } //Optional
    public string Status { get; set; } = "Pending";
}