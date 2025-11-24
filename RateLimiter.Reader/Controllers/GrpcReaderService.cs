using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RateLimiter.Reader.Services;

namespace RateLimiter.Reader.Controllers;

public class GrpcReaderService : Reader.ReaderBase
{
    private readonly IRateLimitService _rateLimitService;
    private readonly RateLimitMapper _mapper;

    public GrpcReaderService(IRateLimitService rateLimitService, RateLimitMapper mapper)
    {
        _rateLimitService = rateLimitService;
        _mapper = mapper;
    }

    public override Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
    {
        return Task.FromResult(new PingResponse
        {
            Status = "Alive"
        });
    }

    public override Task<RateLimitListResponse> GetAllLimits(Empty request, ServerCallContext context)
    {
        var limits = _rateLimitService.GetAllLimits();

        var response = _mapper.ToListResponse(limits);
        return Task.FromResult(response);
    }
}