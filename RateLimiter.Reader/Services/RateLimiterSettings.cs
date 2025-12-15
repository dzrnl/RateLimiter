namespace RateLimiter.Reader.Services;

public class RateLimiterSettings
{
    public int BlockDurationMinutes { get; set; }

    public TimeSpan BlockDuration => TimeSpan.FromMinutes(BlockDurationMinutes);
}