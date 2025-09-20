using FluentValidation;
using UserService.Controllers.Validators;

namespace UserService.Controllers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection collection)
    {
        collection.AddSingleton<UserMapper>();

        collection.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        collection.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();

        return collection;
    }
}