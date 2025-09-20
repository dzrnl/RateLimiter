using UserService.Repositories;
using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private static readonly SemaphoreSlim CreateUserSemaphore = new(1, 1);

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserModel> CreateAsync(CreateUserDto dto)
    {
        await CreateUserSemaphore.WaitAsync();
        try
        {
            var existingUser = await _userRepository.GetUserByLoginAsync(dto.Login);
            if (existingUser != null)
            {
                throw new LoginConflictException();
            }

            return await _userRepository.CreateUserAsync(dto);
        }
        finally
        {
            CreateUserSemaphore.Release();
        }
    }

    public async Task<UserModel> GetByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user is null)
        {
            throw UserNotFoundException.For(nameof(user.Id), userId);
        }

        return user;
    }

    public async Task<UserModel[]> GetByNameAsync(string name, string surname)
    {
        return await _userRepository.GetUsersByNameAsync(name, surname);
    }

    public async Task<UserModel> UpdateAsync(UpdateUserDto dto)
    {
        var updatedUser = await _userRepository.UpdateUserAsync(dto);

        if (updatedUser is null)
        {
            throw UserNotFoundException.For(nameof(dto.Id), dto.Id);
        }

        return updatedUser;
    }

    public async Task<int> DeleteAsync(int userId)
    {
        var deletedId = await _userRepository.DeleteUserAsync(userId);

        if (deletedId is null)
        {
            throw UserNotFoundException.For(nameof(userId), userId);
        }

        return userId;
    }
}