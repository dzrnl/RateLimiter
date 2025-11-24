namespace RateLimiter.Reader.Services;

public class RateLimitHostedService : IHostedService
{
    private readonly IRateLimitService _rateLimitService;
    private readonly ILogger<RateLimitHostedService> _logger;

    public RateLimitHostedService(
        IRateLimitService rateLimitService,
        ILogger<RateLimitHostedService> logger)
    {
        _rateLimitService = rateLimitService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initializing rate limits cache...");

        await _rateLimitService.LoadInitialCacheAsync();

        _logger.LogInformation("Rate limits cache initialized successfully");

        _rateLimitService.StartWatchingForUpdates();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}