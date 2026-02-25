namespace TaskFlow.Api.DTOs;


public record RegisterDto(
    string Email,
    string Password
);