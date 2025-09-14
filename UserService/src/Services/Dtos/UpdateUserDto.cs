namespace UserService.Services.Dtos;

public record UpdateUserDto(
    int Id,
    string? Password,
    string? Name,
    string? Surname,
    int? Age
);