using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public interface IUserService
{
    Task<UserModel> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);

    Task<UserModel> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<UserModel[]> FindUsersByNameAsync(string name, string surname, CancellationToken cancellationToken = default);

    Task<UserModel> UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken = default);

    Task<int> DeleteUserAsync(int userId, CancellationToken cancellationToken = default);
}