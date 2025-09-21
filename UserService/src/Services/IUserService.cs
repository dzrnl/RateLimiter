using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public interface IUserService
{
    Task<UserModel> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default);

    Task<UserModel> GetByIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<UserModel[]> GetByNameAsync(string name, string surname, CancellationToken cancellationToken = default);

    Task<UserModel> UpdateAsync(UpdateUserDto dto, CancellationToken cancellationToken = default);

    Task<int> DeleteAsync(int userId, CancellationToken cancellationToken = default);
}