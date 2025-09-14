using System;

namespace UserService.Services.Exceptions;

public class UserNotFoundExceptionException : Exception
{
    public UserNotFoundException(string message) : base(message);
}