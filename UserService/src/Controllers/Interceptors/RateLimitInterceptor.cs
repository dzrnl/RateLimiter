using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Options;
using UserService.Services;

namespace UserService.Controllers.Interceptors;

public class RateLimitInterceptor : Interceptor
{
    private readonly IUserRateLimitService _rateLimitService;
    private readonly string _userIdHeader;

    public RateLimitInterceptor(IUserRateLimitService rateLimitService, IOptions<GrpcSettings> options)
    {
        _rateLimitService = rateLimitService;
        _userIdHeader = options.Value.UserIdHeader;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var header = context.RequestHeaders.FirstOrDefault(h => h.Key == _userIdHeader)!;

        if (!int.TryParse(header.Value, out var userId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                $"Invalid {_userIdHeader} value: {header.Value}"));
        }

        var endpoint = context.Method;

        if (await _rateLimitService.IsBlockedAsync(userId, endpoint))
        {
            throw new RpcException(new Status(StatusCode.ResourceExhausted,
                $"Rate limit exceeded for user {userId} on endpoint {endpoint}"));
        }

        return await continuation(request, context);
    }
}