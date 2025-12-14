namespace UserService.Services.Configuration;

public class CacheSettings
{
    public int AbsoluteExpirationMinutes { get; init; } = 60;
    public int SlidingExpirationMinutes { get; init; } = 10;

    public TimeSpan AbsoluteExpiration => TimeSpan.FromMinutes(AbsoluteExpirationMinutes);
    public TimeSpan SlidingExpiration => TimeSpan.FromMinutes(SlidingExpirationMinutes);
}