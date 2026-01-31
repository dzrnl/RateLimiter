using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<IUserModel> AddAsync(ICreateUserDto dto, CancellationToken cancellationToken);

    Task<IUserModel?> FindByIdAsync(int userId, CancellationToken cancellationToken);

    Task<IUserModel?> FindByLoginAsync(string login, CancellationToken cancellationToken);

    Task<IUserModel[]> FindAllByNameAsync(string name, string surname, CancellationToken cancellationToken);

    Task<IUserModel?> UpdateAsync(IUpdateUserDto dto, CancellationToken cancellationToken);

    Task<int?> DeleteAsync(int userId, CancellationToken cancellationToken);
}