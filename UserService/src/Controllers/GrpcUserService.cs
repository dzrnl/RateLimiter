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

    public override Task<UserId> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<User> GetUserById(UserId request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<User> GetUserByName(UserFullName request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<UserId> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<UserId> DeleteUser(UserId request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}