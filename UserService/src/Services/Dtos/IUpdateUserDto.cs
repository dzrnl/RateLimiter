namespace UserService.Services.Dtos;

public interface IUpdateUserDto
{
    int Id { get; }

    string? Password { get; }

    string? Name { get; }

    string? Surname { get; }

    int? Age { get; }
}