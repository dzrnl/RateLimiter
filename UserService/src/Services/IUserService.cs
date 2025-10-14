using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public interface IUserService
{
    Task<UserModel> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken);

    Task<UserModel> GetUserByIdAsync(int userId, CancellationToken cancellationToken);

    Task<UserModel[]> FindUsersByNameAsync(string name, string surname, CancellationToken cancellationToken);

    Task<UserModel> UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken);

    Task<int> DeleteUserAsync(int userId, CancellationToken cancellationToken);
}