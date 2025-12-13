using UserService.Repositories;

namespace UserService.Services;

public interface IUserRateLimitService
{
    Task<bool> IsBlockedAsync(int userId, string endpoint);
}

public class UserRateLimitService : IUserRateLimitService
{
    private readonly IUserRateLimitRepository _repository;

    public UserRateLimitService(IUserRateLimitRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> IsBlockedAsync(int userId, string endpoint)
        => _repository.IsUserBlockedAsync(userId, endpoint);
}