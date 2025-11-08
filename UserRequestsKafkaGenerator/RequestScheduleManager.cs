using System.Collections.Concurrent;
using UserRequestsKafkaGenerator.Models;

// ReSharper disable PossiblyMistakenUseOfCancellationToken

namespace UserRequestsKafkaGenerator;

public interface IRequestScheduleManager
{
    Task StartOrUpdateScheduleAsync(RequestSchedule schedule, CancellationToken cancellationToken);
    Task StopScheduleAsync(int userId, string endpoint, CancellationToken cancellationToken);
    Task StopAllSchedulesAsync(CancellationToken cancellationToken);
    IReadOnlyCollection<RequestSchedule> GetActiveSchedules();
}

public record ScheduleKey(int UserId, string Endpoint);

public record ScheduleEntry(CancellationTokenSource Cts, Task Task, RequestSchedule Schedule);

public class RequestScheduleManager : IRequestScheduleManager
{
    private readonly IKafkaProducer _kafkaProducer;
    private readonly ILogger<RequestScheduleManager> _logger;
    private readonly ConcurrentDictionary<ScheduleKey, ScheduleEntry> _activeSchedules = new();

    public RequestScheduleManager(IKafkaProducer kafkaProducer, ILogger<RequestScheduleManager> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task StartOrUpdateScheduleAsync(RequestSchedule schedule, CancellationToken cancellationToken)
    {
        var key = new ScheduleKey(schedule.UserId, schedule.Endpoint);

        if (_activeSchedules.TryGetValue(key, out var entry))
        {
            entry.Cts.Cancel();
            await entry.Task;
        }

        var (cts, task) = CreateScheduleTask(schedule, cancellationToken);
        _activeSchedules[key] = new ScheduleEntry(
            Cts: cts,
            Task: task,
            Schedule: schedule
        );

        var message = entry != null ? "Updated" : "Started";
        _logger.LogInformation("{Message} schedule for user {UserId}: {Rpm} RPM, endpoint: {Endpoint}",
            message, schedule.UserId, schedule.Rpm, schedule.Endpoint);
    }

    public async Task StopScheduleAsync(int userId, string endpoint, CancellationToken cancellationToken)
    {
        var key = new ScheduleKey(userId, endpoint);

        if (!_activeSchedules.TryRemove(key, out var entry))
        {
            _logger.LogInformation("No schedule for user {UserId} with endpoint {Endpoint}", userId, endpoint);
            return;
        }

        entry.Cts.Cancel();
        await entry.Task;

        _logger.LogInformation("Stopped schedule for user {UserId} with endpoint {Endpoint}", userId, endpoint);
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

    public IReadOnlyCollection<RequestSchedule> GetActiveSchedules()
        => _activeSchedules
            .Select(e => e.Value.Schedule)
            .ToList();

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