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

    public async Task BlockUserAsync(int userId, string endpoint, TimeSpan blockDuration)
    {
        await _redisClient.SetBlockFlagAsync(userId, endpoint, blockDuration);
    }

    public async Task<bool> IsUserBlockedAsync(int userId, string endpoint)
    {
        return await _redisClient.GetBlockFlagAsync(userId, endpoint);
    }
}