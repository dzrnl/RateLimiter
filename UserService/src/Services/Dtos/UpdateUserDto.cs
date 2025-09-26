namespace UserService.Services.Dtos;

public sealed record UpdateUserDto(
    int Id,
    string? Password,
    string? Name,
    string? Surname,
    int? Age
);