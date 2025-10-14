using FluentValidation;

namespace RateLimiter.Writer.Controllers.Validators;

public class CreateRateLimitRequestValidator : AbstractValidator<CreateRateLimitRequest>
{
    public CreateRateLimitRequestValidator()
    {
        RuleFor(x => x.Route)
            .NotEmpty().WithMessage("Route is required")
            .Matches(@"^[a-zA-Z_][a-zA-Z0-9_]*(\.[a-zA-Z_][a-zA-Z0-9_]*)+$")
            .WithMessage("Route must be a valid gRPC method name. E.g. ServiceName.MethodName or namespace.Service.Method");

        RuleFor(x => x.RequestsPerMinute)
            .GreaterThan(0).WithMessage("Requests per minute must be greater than 0");
    }
}