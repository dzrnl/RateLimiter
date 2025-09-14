using Grpc.Core;
using UserService.Services;
using static UserService.UserService;

namespace UserService.Controllers;

public class GrpcUserService : UserServiceBase
{
    private readonly IUserService _userService;
    private readonly ILogger<GrpcUserService> _logger;

    public GrpcUserService(IUserService userService, ILogger<GrpcUserService> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public override Task<UserIdResponse> CreateUser(User request, ServerCallContext context)
    {
        _logger.LogInformation("Saying hello to {Name}", request.Name);

        return Task.FromResult(new UserIdResponse
        {
            Id = 0
        });
    }
}