namespace UserService.Services.Models;

public record UserModel(
    int Id,
    string Login,
    string Password,
    string Name,
    string Surname,
    int Age
);