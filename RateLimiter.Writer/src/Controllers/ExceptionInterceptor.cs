using Grpc.Core;
using Grpc.Core.Interceptors;
using RateLimiter.Writer.Services;

namespace RateLimiter.Writer.Controllers;

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
    
    private RpcException HandleException(Exception ex, object request)
    {
        switch (ex)
        {
            case RateLimitAlreadyExistsException ae:
                _logger.LogWarning("Rate limit already exists: {@Request}. Message: {Message}", request, ae.Message);
                return new RpcException(new Status(StatusCode.AlreadyExists, ae.Message));

            case RateLimitNotFoundException nf:
                _logger.LogWarning("RateLimit not found for request: {@Request}. Message: {Message}", request, nf.Message);
                return new RpcException(new Status(StatusCode.NotFound, nf.Message));
            
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