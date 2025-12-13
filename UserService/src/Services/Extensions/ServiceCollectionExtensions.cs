namespace UserService.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddSingleton<IUserRateLimitService, UserRateLimitService>();
        collection.AddSingleton<IUserService, UserService>();
        return collection;
    }
}