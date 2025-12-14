using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public interface IUserService
{
    Task<IUserModel> CreateUserAsync(ICreateUserDto dto, CancellationToken cancellationToken);

    Task<IUserModel> GetUserByIdAsync(int userId, CancellationToken cancellationToken);

    Task<IUserModel[]> FindUsersByNameAsync(string name, string surname, CancellationToken cancellationToken);

    Task<IUserModel> UpdateUserAsync(IUpdateUserDto dto, CancellationToken cancellationToken);

    Task<int> DeleteUserAsync(int userId, CancellationToken cancellationToken);
}