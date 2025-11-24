using Microsoft.Extensions.Options;
using UserRequestsKafkaGenerator.Configuration;

namespace UserRequestsKafkaGenerator;

public class KafkaProducerService : IHostedService
{
    private readonly IRequestScheduleManager _scheduleManager;
    private readonly UserScheduleOptions _options;
    private readonly ILogger<KafkaProducerService> _logger;

    public KafkaProducerService(
        IRequestScheduleManager scheduleManager,
        IOptions<UserScheduleOptions> options,
        ILogger<KafkaProducerService> logger)
    {
        _scheduleManager = scheduleManager;
        _options = options.Value;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var schedule in _options.Schedules)
        {
            await _scheduleManager.StartOrUpdateScheduleAsync(schedule, cancellationToken);
        }

        _logger.LogInformation("KafkaProducerService started");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scheduleManager.StopAllSchedulesAsync(cancellationToken);
        _logger.LogInformation("KafkaProducerService stopped");
    }
}