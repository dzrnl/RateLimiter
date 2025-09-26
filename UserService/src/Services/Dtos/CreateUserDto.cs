namespace UserService.Services.Dtos;

public sealed record CreateUserDto(
    string Login,
    string Password,
    string Name,
    string Surname,
    int Age
);