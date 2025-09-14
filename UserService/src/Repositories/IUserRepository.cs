using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<int> CreateUserAsync(CreateUserDto user);

    Task<UserModel> GetUserByIdAsync(int id);

    Task<List<UserModel>> GetAllUsersAsync();

    Task<UserModel> GetUserByNameAsync(string name, string surname);

    Task DeleteUserAsync(int id);

    Task<bool> ExistsUserAsync(int id);

    Task<UserModel> UpdateUserAsync(int id, UpdateUserDto updatableFields);
}