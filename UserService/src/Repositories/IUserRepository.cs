using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<UserModel> AddAsync(CreateUserDto dto, CancellationToken cancellationToken);

    Task<UserModel?> FindByIdAsync(int userId, CancellationToken cancellationToken);

    Task<UserModel?> FindByLoginAsync(string login, CancellationToken cancellationToken);

    Task<UserModel[]> FindAllByNameAsync(string name, string surname, CancellationToken cancellationToken);

    Task<UserModel?> UpdateAsync(UpdateUserDto dto, CancellationToken cancellationToken);

    Task<int?> DeleteAsync(int userId, CancellationToken cancellationToken);
}