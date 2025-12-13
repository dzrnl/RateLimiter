namespace RateLimiter.Reader.Repositories;

public interface IRequestCounterRepository
{
    Task<bool> TryConsumeRequestAsync(int userId, string endpoint, int limit);
}

public class RequestCounterRepository : IRequestCounterRepository
{
    private readonly RateLimitsStatisticsRedisClient _redisClient;

    public RequestCounterRepository(RateLimitsStatisticsRedisClient redisClient)
    {
        _redisClient = redisClient;
    }

    private static string CounterKey(int userId, string endpoint)
        => $"rate_limit:counter:{userId}:{endpoint}";

    public async Task<bool> TryConsumeRequestAsync(int userId, string endpoint, int limit)
    {
        var ttl = TimeSpan.FromMinutes(1);

        var key = CounterKey(userId, endpoint);
        var currentCount = await _redisClient.IncrementCounterAsync(key, ttl);

        return currentCount <= limit;
    }
}