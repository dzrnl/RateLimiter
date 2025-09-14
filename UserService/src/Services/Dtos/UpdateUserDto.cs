namespace UserService.Services.Dtos;

public record UpdateUserDto(
    string Password,
    string Name,
    string Surname,
    int Age
);