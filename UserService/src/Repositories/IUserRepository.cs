using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<UserModel> AddAsync(CreateUserDto dto, CancellationToken cancellationToken = default);

    Task<UserModel?> FindByIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<UserModel?> FindByLoginAsync(string login, CancellationToken cancellationToken = default);

    Task<UserModel[]> FindAllByNameAsync(string name, string surname, CancellationToken cancellationToken = default);

    Task<UserModel?> UpdateAsync(UpdateUserDto dto, CancellationToken cancellationToken = default);

    Task<int?> DeleteAsync(int userId, CancellationToken cancellationToken = default);
}