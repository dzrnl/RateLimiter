namespace UserService.Services.Dtos;

public interface ICreateUserDto
{
    string Login { get; }

    string Password { get; }

    string Name { get; }

    string Surname { get; }

    int Age { get; }
}