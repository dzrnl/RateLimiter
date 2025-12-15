using Grpc.Core;
using Grpc.Core.Interceptors;
using UserService.Services;

namespace UserService.Controllers.Interceptors;

public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            throw HandleException(ex, request);
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception ex)
        {
            throw HandleException(ex, request);
        }
    }

    private RpcException HandleException(Exception ex, object request)
    {
        switch (ex)
        {
            case LoginConflictException le:
                _logger.LogWarning("Login conflict for request: {@Request}. Message: {Message}", request, le.Message);
                return new RpcException(new Status(StatusCode.AlreadyExists, le.Message));

            case UserNotFoundException ue:
                _logger.LogWarning("User not found for request: {@Request}. Message: {Message}", request, ue.Message);
                return new RpcException(new Status(StatusCode.NotFound, ue.Message));

            case FluentValidation.ValidationException ve:
                _logger.LogWarning("Validation failed for request: {@Request}. Message: {Message}", request, ve.Message);
                return new RpcException(new Status(StatusCode.InvalidArgument, ve.Message));

            case OperationCanceledException oce:
                _logger.LogWarning("Request was cancelled: {@Request}. Message: {Message}", request, oce.Message);
                return new RpcException(new Status(StatusCode.Cancelled, "Request was cancelled"));

            default:
                _logger.LogError(ex, "Unhandled exception for request: {@Request}. Message: {Message}", request, ex.Message);
                return new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }
}