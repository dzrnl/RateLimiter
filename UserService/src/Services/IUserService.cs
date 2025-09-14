using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public interface IUserService
{
    Task<int> Create(CreateUserDto dto);

    Task<UserModel> GetById(int userId);

    Task<UserModel> GetByName(string name, string surname);

    Task<int> Update(UpdateUserDto dto);

    Task<int> Delete(int userId);
}