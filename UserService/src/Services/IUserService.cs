using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public interface IUserService
{
    Task<UserModel> CreateAsync(CreateUserDto dto);

    Task<UserModel> GetByIdAsync(int userId);

    Task<UserModel[]> GetByNameAsync(string name, string surname);

    Task<UserModel> UpdateAsync(UpdateUserDto dto);

    Task<int> DeleteAsync(int userId);
}