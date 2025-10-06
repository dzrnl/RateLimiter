using RateLimiter.Writer.Repositories.Extensions;
using RateLimiter.Writer.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddApplication();
builder.Services.AddInfrastructureDataAccess(builder.Configuration);

var app = builder.Build();

await app.RunAsync("http://*:5001");