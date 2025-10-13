using RateLimiter.Writer.Controllers;
using RateLimiter.Writer.Controllers.Extensions;
using RateLimiter.Writer.Repositories.Extensions;
using RateLimiter.Writer.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddApplication();
builder.Services.AddInfrastructureDataAccess(builder.Configuration);
builder.Services.AddGrpcServices();

var app = builder.Build();

app.MapGrpcService<GrpcWriterService>();

await app.RunAsync("http://*:5001");