using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UserRequestsKafkaGenerator;

public class KafkaProducerService : IHostedService
{
    private readonly IRequestScheduleManager _scheduleManager;
    private readonly ILogger<KafkaProducerService> _logger;

    public KafkaProducerService(IRequestScheduleManager scheduleManager, ILogger<KafkaProducerService> logger)
    {
        _scheduleManager = scheduleManager;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _scheduleManager.StartOrUpdateScheduleAsync(new RequestSchedule(123, "GetUserById", 10), cancellationToken);
        await _scheduleManager.StartOrUpdateScheduleAsync(new RequestSchedule(321, "GetUserById", 5), cancellationToken);

        _logger.LogInformation("KafkaProducerService started");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scheduleManager.StopAllSchedulesAsync(cancellationToken);
        
        _logger.LogInformation("KafkaProducerService stopped");
    }
}