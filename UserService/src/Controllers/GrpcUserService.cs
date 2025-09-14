using FluentValidation;
using Grpc.Core;
using UserService.Services;
using static UserService.UserService;

namespace UserService.Controllers;

public class GrpcUserService : UserServiceBase
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _createValidator;
    private readonly IValidator<UpdateUserRequest> _updateValidator;
    private readonly ILogger<GrpcUserService> _logger;

    public GrpcUserService(
        IUserService userService,
        IValidator<CreateUserRequest> createValidator,
        IValidator<UpdateUserRequest> updateValidator,
        ILogger<GrpcUserService> logger)
    {
        _userService = userService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    public override async Task<UserId> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation("CreateUser called. Login: {Login}, Name: {Name}, Surname: {Surname}, Age: {Age}",
            request.Login, request.Name, request.Surname, request.Age);

        await _createValidator.ValidateAndThrowAsync(request);

        throw new NotImplementedException();
    }

    public override Task<User> GetUserById(UserId request, ServerCallContext context)
    {
        _logger.LogInformation("GetUserById called. UserId: {Id}", request.Id);
        throw new NotImplementedException();
    }

    public override Task<User> GetUserByName(UserFullName request, ServerCallContext context)
    {
        _logger.LogInformation("GetUserByName called. Name: {Name}, Surname: {Surname}", request.Name, request.Surname);
        throw new NotImplementedException();
    }

    public override async Task<UserId> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation(
            "UpdateUser called. UserId: {Id}, Password: {Password}, Name: {Name}, Surname: {Surname}, Age: {Age}",
            request.Id,
            request.HasPassword ? request.Password : "null",
            request.HasName ? request.Name : "null",
            request.HasSurname ? request.Surname : "null",
            request.HasAge ? request.Age.ToString() : "null"
        );

        await _updateValidator.ValidateAndThrowAsync(request);

        throw new NotImplementedException();
    }

    public override Task<UserId> DeleteUser(UserId request, ServerCallContext context)
    {
        _logger.LogInformation("DeleteUser called. UserId: {Id}", request.Id);
        throw new NotImplementedException();
    }
}