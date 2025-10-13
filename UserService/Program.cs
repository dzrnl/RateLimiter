using UserService.Controllers;
using UserService.Controllers.Extensions;
using UserService.Repositories.Extensions;
using UserService.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});

builder.Services.AddApplication();
builder.Services.AddInfrastructureDataAccess(builder.Configuration);
builder.Services.AddGrpcServices();

var app = builder.Build();

app.MapGrpcService<GrpcUserService>();

await app.RunAsync("http://*:5002");