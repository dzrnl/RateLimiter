using UserRequestsKafkaGenerator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection(nameof(KafkaSettings)));
builder.Services.Configure<UserScheduleOptions>(builder.Configuration.GetSection(nameof(UserScheduleOptions)));

builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<IRequestScheduleManager, RequestScheduleManager>();
builder.Services.AddHostedService<KafkaProducerService>();

var app = builder.Build();

var manager = app.Services.GetRequiredService<IRequestScheduleManager>();

app.MapPost("/add", async (RequestSchedule schedule, CancellationToken cancellationToken) =>
{
    await manager.StartOrUpdateScheduleAsync(schedule, cancellationToken);
    return Results.Ok();
});

app.MapPost("/remove", async (int userId, string endpoint, CancellationToken cancellationToken) =>
{
    await manager.StopScheduleAsync(userId, endpoint, cancellationToken);
    return Results.Ok();
});

app.MapPost("/remove-all", async (CancellationToken cancellationToken) =>
{
    await manager.StopAllSchedulesAsync(cancellationToken);
    return Results.Ok();
});

app.MapGet("/list", () => manager.GetActiveSchedules());

await app.RunAsync("http://*:5000");