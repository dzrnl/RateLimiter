using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<UserModel> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);

    Task<UserModel?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<UserModel?> GetUserByLoginAsync(string login, CancellationToken cancellationToken = default);

    Task<UserModel[]> GetUsersByNameAsync(string name, string surname, CancellationToken cancellationToken = default);

    Task<UserModel?> UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken = default);

    Task<int?> DeleteUserAsync(int userId, CancellationToken cancellationToken = default);
}