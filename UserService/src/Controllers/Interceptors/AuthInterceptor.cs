using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Options;

namespace UserService.Controllers.Interceptors;

public class AuthInterceptor : Interceptor
{
    private readonly string _userIdHeader;

    public AuthInterceptor(IOptions<GrpcSettings> options)
    {
        _userIdHeader = options.Value.UserIdHeader;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        if (context.RequestHeaders.All(h => h.Key != _userIdHeader))
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated,
                $"Missing required header: {_userIdHeader}"));
        }

        return await continuation(request, context);
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        if (context.RequestHeaders.All(h => h.Key != _userIdHeader))
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated,
                $"Missing required header: {_userIdHeader}"));
        }

        await continuation(request, responseStream, context);
    }
}