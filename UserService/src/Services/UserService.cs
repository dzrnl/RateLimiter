using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using UserService.Repositories;
using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;

    private static string UserByIdKey(int id) => $"user:id:{id}";
    private static string UserByNameKey(string name, string surname) => $"user:list:{name}:{surname}";

    public UserService(IUserRepository userRepository, IMemoryCache cache)
    {
        _userRepository = userRepository;
        _cache = cache;
    }

    public async Task<IUserModel> CreateUserAsync(ICreateUserDto dto, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.FindByLoginAsync(dto.Login, cancellationToken);
        if (existingUser != null)
        {
            throw new LoginConflictException();
        }

        var user = await _userRepository.AddAsync(dto, cancellationToken);

        _cache.Set(UserByIdKey(user.Id), user);
        if (_cache.TryGetValue(
                UserByNameKey(user.Name, user.Surname),
                out ConcurrentDictionary<int, IUserModel>? list)
            && list != null)
        {
            list[user.Id] = user;
        }

        return user;
    }

    public async Task<IUserModel> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
    {
        var cacheKey = UserByIdKey(userId);
        if (_cache.TryGetValue(cacheKey, out IUserModel? cached) && cached != null)
        {
            return cached;
        }

        var user = await _userRepository.FindByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw UserNotFoundException.For(nameof(IUserModel.Id), userId);
        }

        _cache.Set(cacheKey, user);
        return user;
    }

    public async Task<IUserModel[]> FindUsersByNameAsync(string name, string surname, CancellationToken cancellationToken)
    {
        var cacheKey = UserByNameKey(name, surname);
        if (_cache.TryGetValue(cacheKey, out ConcurrentDictionary<int, IUserModel>? cached) && cached != null)
        {
            return cached.Values.ToArray();
        }

        var users = await _userRepository.FindAllByNameAsync(name, surname, cancellationToken);

        var dict = new ConcurrentDictionary<int, IUserModel>(
            users.ToDictionary(u => u.Id)
        );

        _cache.Set(cacheKey, dict);
        return users;
    }

    public async Task<IUserModel> UpdateUserAsync(IUpdateUserDto dto, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.FindByIdAsync(dto.Id, cancellationToken);
        if (existingUser is null)
        {
            throw UserNotFoundException.For(nameof(IUserModel.Id), dto.Id);
        }

        var updatedUser = await _userRepository.UpdateAsync(dto, cancellationToken);
        if (updatedUser is null)
        {
            throw UserNotFoundException.For(nameof(IUserModel.Id), dto.Id);
        }

        _cache.Set(UserByIdKey(updatedUser.Id), updatedUser);

        var oldCacheNameKey = UserByNameKey(existingUser.Name, existingUser.Surname);
        if (_cache.TryGetValue(oldCacheNameKey, out ConcurrentDictionary<int, IUserModel>? oldList) && oldList != null)
        {
            oldList.TryRemove(updatedUser.Id, out _);
        }

        var newCacheNameKey = UserByNameKey(updatedUser.Name, updatedUser.Surname);
        if (_cache.TryGetValue(newCacheNameKey, out ConcurrentDictionary<int, IUserModel>? newList) && newList != null)
        {
            newList[updatedUser.Id] = updatedUser;
        }

        return updatedUser;
    }

    public async Task<int> DeleteUserAsync(int userId, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.FindByIdAsync(userId, cancellationToken);
        if (existingUser is null)
        {
            throw UserNotFoundException.For(nameof(IUserModel.Id), userId);
        }

        var deletedId = await _userRepository.DeleteAsync(userId, cancellationToken);
        if (deletedId is null)
        {
            throw UserNotFoundException.For(nameof(IUserModel.Id), userId);
        }

        _cache.Remove(UserByIdKey(userId));

        var cacheNameKey = UserByNameKey(existingUser.Name, existingUser.Surname);
        if (_cache.TryGetValue(cacheNameKey, out ConcurrentDictionary<int, IUserModel>? list) && list != null)
        {
            list.TryRemove(userId, out _);
        }

        return userId;
    }
}