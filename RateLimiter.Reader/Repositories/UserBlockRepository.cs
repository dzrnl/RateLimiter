namespace RateLimiter.Reader.Repositories;

public interface IUserBlockRepository
{
    Task BlockUserAsync(int userId, string endpoint, TimeSpan blockDuration);

    Task<bool> IsUserBlockedAsync(int userId, string endpoint);
}

public class UserBlockRepository : IUserBlockRepository
{
    private readonly RateLimitsStatisticsRedisClient _redisClient;

    public UserBlockRepository(RateLimitsStatisticsRedisClient redisClient)
    {
        _redisClient = redisClient;
    }

    private static string BlockKey(int userId, string endpoint)
        => $"rate_limit:block:{userId}:{endpoint}";

    public async Task BlockUserAsync(int userId, string endpoint, TimeSpan blockDuration)
    {
        var key = BlockKey(userId, endpoint);
        await _redisClient.SetFlagAsync(key, blockDuration);
    }

    public async Task<bool> IsUserBlockedAsync(int userId, string endpoint)
    {
        var key = BlockKey(userId, endpoint);
        return await _redisClient.KeyExistsAsync(key);
    }
}