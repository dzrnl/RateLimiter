namespace RateLimiter.Reader.Repositories.Configuration;

public class RedisSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public int BlockExpirationMinutes { get; set; }
}