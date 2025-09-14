using System;

namespace UserService.Services.Exceptions;

public class LoginConflictException : Exception
{
    public LoginConflictException(string message) : base(message);
}