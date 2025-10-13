using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RateLimiter.Writer.Services;

namespace RateLimiter.Writer.Controllers;

public class GrpcWriterService : Writer.WriterBase
{
    private readonly IRateLimitService _rateLimitService;
    private readonly RateLimitMapper _mapper;

    public GrpcWriterService(
        IRateLimitService rateLimitService,
        RateLimitMapper mapper)
    {
        _rateLimitService = rateLimitService;
        _mapper = mapper;
    }

    public override Task<Empty> CreateLimit(CreateRateLimitRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<RateLimitResponse> GetLimitByRoute(RouteRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<RateLimitResponse> UpdateLimit(UpdateRateLimitRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }

    public override Task<Empty> DeleteLimit(RouteRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}