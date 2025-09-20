using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public interface IUserService
{
    Task<UserModel> Create(CreateUserDto dto);

    Task<UserModel> GetById(int userId);

    Task<UserModel[]> GetByName(string name, string surname);

    Task<UserModel> Update(UpdateUserDto dto);

    Task<int> Delete(int userId);
}