using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RateLimiter.Writer.Services;

namespace RateLimiter.Writer.Controllers;

public class GrpcWriterService : Writer.WriterBase
{
    private readonly IRateLimitService _rateLimitService;
    private readonly RateLimitMapper _mapper;

    private readonly IValidator<CreateRateLimitRequest> _createValidator;
    private readonly IValidator<UpdateRateLimitRequest> _updateValidator;

    public GrpcWriterService(
        IRateLimitService rateLimitService,
        RateLimitMapper mapper,
        IValidator<CreateRateLimitRequest> createValidator,
        IValidator<UpdateRateLimitRequest> updateValidator)
    {
        _rateLimitService = rateLimitService;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public override async Task<Empty> CreateLimit(CreateRateLimitRequest request, ServerCallContext context)
    {
        _createValidator.ValidateAndThrow(request);

        var createModel = _mapper.ToCreateModel(request);
        await _rateLimitService.CreateRateLimitAsync(createModel, context.CancellationToken);

        return new Empty();
    }

    public override async Task<RateLimitResponse> GetLimitByRoute(RouteRequest request, ServerCallContext context)
    {
        var rateLimit = await _rateLimitService.GetRateLimitByRouteAsync(request.Route, context.CancellationToken);
        return _mapper.ToResponse(rateLimit);
    }

    public override async Task<RateLimitResponse> UpdateLimit(UpdateRateLimitRequest request, ServerCallContext context)
    {
        _updateValidator.ValidateAndThrow(request);

        var updateModel = _mapper.ToUpdateModel(request);
        var rateLimit = await _rateLimitService.UpdateRateLimitAsync(updateModel, context.CancellationToken);

        return _mapper.ToResponse(rateLimit);
    }

    public override async Task<Empty> DeleteLimit(RouteRequest request, ServerCallContext context)
    {
        await _rateLimitService.DeleteRateLimitAsync(request.Route, context.CancellationToken);
        return new Empty();
    }
}