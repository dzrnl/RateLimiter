using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Repositories;

 public interface IUserRepository
{
    Task<int> CreateUserAsync(CreateUserDto user);
    Task<User> GetUserByIdAsync(int id);
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByNameAsync(string name);
    Task DeleteUserAsync(int id);
    Task<bool> ExistsUserAsync(int id);
    Task<User> UpdateUserAsync(int id, UpdateUserDto updatableFields);
}