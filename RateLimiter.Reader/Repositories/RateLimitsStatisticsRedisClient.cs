using StackExchange.Redis;

namespace RateLimiter.Reader.Repositories
{
    public class RateLimitsStatisticsRedisClient
    {
        private readonly IDatabase _database;

        public RateLimitsStatisticsRedisClient(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<long> IncrementCounterAsync(int userId, string endpoint, TimeSpan ttl)
        {
            var key = GenerateCounterKey(userId, endpoint);

            var result = await _database.StringIncrementAsync(key);

            if (result == 1)
            {
                await _database.KeyExpireAsync(key, ttl);
            }

            return result;
        }

        public async Task SetBlockFlagAsync(int userId, string endpoint, TimeSpan blockDuration)
        {
            var key = GenerateBlockKey(userId, endpoint);
            await _database.KeyExpireAsync(key, blockDuration);
        }

        public async Task<bool> GetBlockFlagAsync(int userId, string endpoint)
        {
            var key = GenerateBlockKey(userId, endpoint);
            return await _database.KeyExistsAsync(key);
        }

        private static string GenerateCounterKey(int userId, string endpoint)
            => $"rate_limit:counter:{userId}:{endpoint}";

        private static string GenerateBlockKey(int userId, string endpoint)
            => $"rate_limit:block:{userId}:{endpoint}";
    }
}