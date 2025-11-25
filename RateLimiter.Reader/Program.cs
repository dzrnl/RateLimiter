using RateLimiter.Reader.Controllers;
using RateLimiter.Reader.Controllers.Extensions;
using RateLimiter.Reader.Kafka.Extensions;
using RateLimiter.Reader.Repositories.Extensions;
using RateLimiter.Reader.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddApplication();
builder.Services.AddInfrastructureDataAccess(builder.Configuration);
builder.Services.AddKafkaConsumer(builder.Configuration);
builder.Services.AddGrpcServices();

var app = builder.Build();

app.MapGrpcService<GrpcReaderService>();

await app.RunAsync("http://*:5000");