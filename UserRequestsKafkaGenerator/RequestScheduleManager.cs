using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

// ReSharper disable PossiblyMistakenUseOfCancellationToken

namespace UserRequestsKafkaGenerator;

public interface IRequestScheduleManager
{
    Task StartOrUpdateScheduleAsync(RequestSchedule schedule, CancellationToken cancellationToken);
    Task StopScheduleAsync(int userId, CancellationToken cancellationToken);
    Task StopAllSchedulesAsync(CancellationToken cancellationToken);
}

public class RequestScheduleManager : IRequestScheduleManager
{
    private readonly IKafkaProducer _kafkaProducer;
    private readonly ILogger<RequestScheduleManager> _logger;
    private readonly ConcurrentDictionary<int, (CancellationTokenSource Cts, Task Task)> _activeSchedules = new();

    public RequestScheduleManager(IKafkaProducer kafkaProducer, ILogger<RequestScheduleManager> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task StartOrUpdateScheduleAsync(RequestSchedule schedule, CancellationToken cancellationToken)
    {
        if (_activeSchedules.TryGetValue(schedule.UserId, out var entry))
        {
            entry.Cts.Cancel();
            await entry.Task;
        }

        var (cts, task) = CreateScheduleTask(schedule, cancellationToken);
        _activeSchedules[schedule.UserId] = (cts, task);

        var message = entry.Cts != null ? "Updated" : "Started";
        _logger.LogInformation("{Message} schedule for user {UserId}: {Rpm} RPM, endpoint: {Endpoint}",
            message, schedule.UserId, schedule.Rpm, schedule.Endpoint);
    }

    public async Task StopScheduleAsync(int userId, CancellationToken cancellationToken)
    {
        if (!_activeSchedules.TryRemove(userId, out var entry))
        {
            _logger.LogInformation("No schedule for user {UserId}", userId);
            return;
        }

        entry.Cts.Cancel();
        await entry.Task;

        _logger.LogInformation("Stopped schedule for user {UserId}", userId);
    }

    public async Task StopAllSchedulesAsync(CancellationToken cancellationToken)
    {
        foreach (var (_, entry) in _activeSchedules)
        {
            entry.Cts.Cancel();
            await entry.Task;
        }

        _activeSchedules.Clear();
        _logger.LogInformation("Stopped all schedules");
    }

    private (CancellationTokenSource, Task) CreateScheduleTask(
        RequestSchedule schedule,
        CancellationToken cancellationToken)
    {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var task = Task.Factory.StartNew(
            async () => await RunScheduleAsync(schedule, cts.Token),
            cancellationToken,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);

        return (cts, task.Unwrap());
    }

    private async Task RunScheduleAsync(RequestSchedule schedule, CancellationToken cancellationToken)
    {
        var interval = TimeSpan.FromSeconds(60.0 / schedule.Rpm);
        var timer = new PeriodicTimer(interval);

        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                var request = new UserRequest(schedule.UserId, schedule.Endpoint);
                await _kafkaProducer.ProduceAsync(request, cancellationToken);
                _logger.LogDebug("Sent request for user {UserId} to {Endpoint}", schedule.UserId, schedule.Endpoint);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Schedule for user {UserId} was stopped", schedule.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during schedule for user {UserId}", schedule.UserId);
        }
    }
}