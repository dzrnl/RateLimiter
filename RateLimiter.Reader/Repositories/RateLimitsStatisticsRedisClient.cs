using StackExchange.Redis;

namespace RateLimiter.Reader.Repositories;

public class RateLimitsStatisticsRedisClient
{
    private readonly IDatabase _database;

    public RateLimitsStatisticsRedisClient(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<long> IncrementCounterAsync(string key, TimeSpan ttl)
    {
        var result = await _database.StringIncrementAsync(key);

        if (result == 1)
        {
            await _database.KeyExpireAsync(key, ttl);
        }

        return result;
    }

    public async Task SetFlagAsync(string key, TimeSpan ttl)
    {
        await _database.StringSetAsync(key, 1, ttl);
    }

    public Task<bool> KeyExistsAsync(string key)
    {
        return _database.KeyExistsAsync(key);
    }
}