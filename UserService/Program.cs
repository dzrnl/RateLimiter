using UserService.Controllers;
using UserService.Controllers.Extensions;
using UserService.Controllers.Interceptors;
using UserService.Repositories.Extensions;
using UserService.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<AuthInterceptor>();
    options.Interceptors.Add<RateLimitInterceptor>();
    options.Interceptors.Add<ExceptionInterceptor>();
});

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddInfrastructureDataAccess(builder.Configuration);
builder.Services.AddGrpcServices(builder.Configuration);

var app = builder.Build();

app.MapGrpcService<GrpcUserService>();

await app.RunAsync("http://*:5002");