namespace RateLimiter.Reader.Repositories;


public interface IRequestCounterRepository
{
    Task<bool> IsRequestAllowedAsync(int userId, string endpoint, int limit);
}

public class RequestCounterRepository : IRequestCounterRepository
{
    private readonly RateLimitsStatisticsRedisClient _redisClient;

    public RequestCounterRepository(RateLimitsStatisticsRedisClient redisClient)
    {
        _redisClient = redisClient;
    }

    public async Task<bool> IsRequestAllowedAsync(int userId, string endpoint, int limit)
    {
        var ttl = TimeSpan.FromMinutes(1);
        
        var currentCount = await _redisClient.IncrementCounterAsync(userId, endpoint, ttl);
        
        return currentCount <= limit;
    }
}