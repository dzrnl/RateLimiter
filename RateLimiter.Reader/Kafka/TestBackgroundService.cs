namespace RateLimiter.Reader.Kafka;

public class TestBackgroundService : BackgroundService
{
    private readonly ILogger<TestBackgroundService> _logger;

    public TestBackgroundService(ILogger<TestBackgroundService> logger) =>
        _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogWarning("TestBackgroundService is running...");
            await Task.Delay(5000, stoppingToken);
        }
    }
}