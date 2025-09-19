using UserService.Repositories;
using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> Create(CreateUserDto dto)
    {
        if (await _userRepository.GetUserByLoginAsync(dto.Login) != null)
        {
            throw new LoginConflictException();
        }

        var createdUser = await _userRepository.CreateUserAsync(dto);

        return createdUser.Id;
    }

    public async Task<UserModel> GetById(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user is null)
        {
            throw UserNotFoundException.For(nameof(user.Id), userId);
        }

        return user;
    }

    public async Task<UserModel> GetByName(string name, string surname)
    {
        var user = await _userRepository.GetUserByNameAsync(name, surname);

        if (user is null)
        {
            throw UserNotFoundException.For(nameof(user.Name) + " " + nameof(user.Surname), name + " " + surname);
        }

        return user;
    }

    public async Task<int> Update(UpdateUserDto dto)
    {
        // TODO: must throw UserNotFoundException
        return await _userRepository.UpdateUserAsync(dto);
    }

    public async Task<int> Delete(int userId)
    {
        var deletedId = await _userRepository.DeleteUserAsync(userId);

        if (deletedId is null)
        {
            throw UserNotFoundException.For(nameof(userId), userId);
        }

        return userId;
    }
}