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

    public async Task<UserModel> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken)
    {
        await CreateUserSemaphore.WaitAsync(cancellationToken);
        try
        {
            var existingUser = await _userRepository.GetUserByLoginAsync(dto.Login, cancellationToken);
            if (existingUser != null)
            {
                throw new LoginConflictException();
            }

            return await _userRepository.CreateUserAsync(dto, cancellationToken);
        }
        finally
        {
            CreateUserSemaphore.Release();
        }
    }

    public async Task<UserModel> GetByIdAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw UserNotFoundException.For(nameof(user.Id), userId);
        }

        return user;
    }

    public async Task<UserModel[]> GetByNameAsync(string name, string surname, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUsersByNameAsync(name, surname, cancellationToken);
    }

    public async Task<UserModel> UpdateAsync(UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var updatedUser = await _userRepository.UpdateUserAsync(dto, cancellationToken);

        if (updatedUser is null)
        {
            throw UserNotFoundException.For(nameof(dto.Id), dto.Id);
        }

        return updatedUser;
    }

    public async Task<int> DeleteAsync(int userId, CancellationToken cancellationToken)
    {
        var deletedId = await _userRepository.DeleteUserAsync(userId, cancellationToken);

        if (deletedId is null)
        {
            throw UserNotFoundException.For(nameof(userId), userId);
        }

        return userId;
    }
}