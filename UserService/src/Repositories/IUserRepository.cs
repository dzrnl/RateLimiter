using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<UserModel> AddAsync(CreateUserDto dto, CancellationToken cancellationToken = default); // лучше Add

    Task<UserModel?> GetByIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<UserModel?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);

    Task<UserModel[]> FindByNameAsync(string name, string surname, CancellationToken cancellationToken = default);

    Task<UserModel?> UpdateAsync(UpdateUserDto dto, CancellationToken cancellationToken = default);

    Task<int?> DeleteAsync(int userId, CancellationToken cancellationToken = default);
}