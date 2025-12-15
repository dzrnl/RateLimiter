using StackExchange.Redis;

namespace UserService.Repositories;

public interface IUserRateLimitRepository
{
    Task<bool> IsUserBlockedAsync(int userId, string endpoint);
}

public class UserRateLimitRepository : IUserRateLimitRepository
{
    private readonly IDatabase _redis;

    public UserRateLimitRepository(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    private static string BlockKey(int userId, string endpoint)
        => $"rate_limit:block:{userId}:{endpoint}";

    public Task<bool> IsUserBlockedAsync(int userId, string endpoint)
    {
        var key = BlockKey(userId, endpoint);
        return _redis.KeyExistsAsync(key);
    }
}