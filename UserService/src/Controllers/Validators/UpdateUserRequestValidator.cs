using FluentValidation;

namespace UserService.Controllers.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        When(x => x.HasPassword, () => {
            RuleFor(x => x.Password)
                .MinimumLength(4).WithMessage("Password must be at least 4 characters long");
        });

        When(x => x.HasName, () => {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name cannot be empty");
        });

        When(x => x.HasSurname, () => {
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname cannot be empty");
        });

        When(x => x.HasAge, () => {
            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("Age must be greater than 0");
        });

        RuleFor(x => x)
            .Must(x => x.HasPassword || x.HasName || x.HasSurname || x.HasAge)
            .WithMessage("At least one field must be provided for update");
    }
}