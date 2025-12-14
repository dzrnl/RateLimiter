namespace UserService.Services.Models;

public interface IUserModel
{
    int Id { get; }

    string Login { get; }

    string Password { get; }

    string Name { get; }

    string Surname { get; }

    int Age { get; }
}