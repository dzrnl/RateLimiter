using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<int> CreateUserAsync(CreateUserDto user);

    Task<UserModel?> GetUserByIdAsync(int id);

    Task<UserModel?> GetUserByNameAsync(string name, string surname);

    Task<UserModel?> GetUserByLoginAsync(string login);

    Task<int?> DeleteUserAsync(int id);

    Task<int> UpdateUserAsync(UpdateUserDto dto);
}