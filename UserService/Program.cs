using UserService.Controllers;
using UserService.Controllers.Extensions;
using UserService.Repositories.Extensions;
using UserService.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddApplication();
builder.Services.AddInfrastructureDataAccess();
builder.Services.AddValidators();

var app = builder.Build();

app.MapGrpcService<GrpcUserService>();

await app.RunAsync("http://*:5002");