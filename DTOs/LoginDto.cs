namespace TaskFlow.Api.DTOs;

public record LoginDto(
    string Email,
    string Password
);