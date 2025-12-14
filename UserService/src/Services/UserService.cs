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

    public async Task<IUserModel> CreateUserAsync(ICreateUserDto dto, CancellationToken cancellationToken)
    {
        await CreateUserSemaphore.WaitAsync(cancellationToken);
        try
        {
            var existingUser = await _userRepository.FindByLoginAsync(dto.Login, cancellationToken);
            if (existingUser != null)
            {
                throw new LoginConflictException();
            }

            return await _userRepository.AddAsync(dto, cancellationToken);
        }
        finally
        {
            CreateUserSemaphore.Release();
        }
    }

    public async Task<IUserModel> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw UserNotFoundException.For(nameof(IUserModel.Id), userId);
        }

        return user;
    }

    public Task<IUserModel[]> FindUsersByNameAsync(string name, string surname, CancellationToken cancellationToken)
    {
        return _userRepository.FindAllByNameAsync(name, surname, cancellationToken);
    }

    public async Task<IUserModel> UpdateUserAsync(IUpdateUserDto dto, CancellationToken cancellationToken)
    {
        var updatedUser = await _userRepository.UpdateAsync(dto, cancellationToken);

        if (updatedUser is null)
        {
            throw UserNotFoundException.For(nameof(IUserModel.Id), dto.Id);
        }

        return updatedUser;
    }

    public async Task<int> DeleteUserAsync(int userId, CancellationToken cancellationToken)
    {
        var deletedId = await _userRepository.DeleteAsync(userId, cancellationToken);

        if (deletedId is null)
        {
            throw UserNotFoundException.For(nameof(IUserModel.Id), userId);
        }

        return userId;
    }
}