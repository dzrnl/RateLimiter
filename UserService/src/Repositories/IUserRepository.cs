using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<UserModel> CreateUserAsync(CreateUserDto dto);

    Task<UserModel?> GetUserByIdAsync(int userId);

    Task<UserModel?> GetUserByLoginAsync(string login);

    Task<IEnumerable<UserModel>> GetUserByNameAsync(string name, string surname);

    Task<int> UpdateUserAsync(UpdateUserDto dto);

    Task<int?> DeleteUserAsync(int userId);
}