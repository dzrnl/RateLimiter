using UserService.Repositories.Extensions;
using UserService.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddApplication();
builder.Services.AddInfrastructureDataAccess();

var app = builder.Build();

app.MapGrpcService<UserService.Services.UserService>();

await app.RunAsync("http://*:5002");