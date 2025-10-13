namespace UserService.Services;

public class LoginConflictException() : Exception("Error: user with this login already exists");

public class UserNotFoundException(string fieldName, string fieldValue)
    : Exception($"Error: no user with such {fieldName} = {fieldValue}")
{
    public static UserNotFoundException For<T>(string fieldName, T fieldValue)
        => new(fieldName, fieldValue?.ToString() ?? "null");
}