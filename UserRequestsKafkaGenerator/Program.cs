using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserRequestsKafkaGenerator;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.Configure<KafkaSettings>(context.Configuration.GetSection(nameof(KafkaSettings)));
        services.AddSingleton<IKafkaProducer, KafkaProducer>();
        services.AddSingleton<IRequestScheduleManager, RequestScheduleManager>();
        services.AddHostedService<KafkaProducerService>();
    })
    .Build();

await host.RunAsync();