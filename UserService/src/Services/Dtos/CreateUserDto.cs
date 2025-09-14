namespace UserService.Services.Dtos;

public record CreateUserDto(
    string Login,
    string Password,
    string Name,
    string Surname,
    int Age
);