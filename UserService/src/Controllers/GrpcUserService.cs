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
    private readonly UserMapper _mapper;

    public GrpcUserService(
        IUserService userService,
        IValidator<CreateUserRequest> createValidator,
        IValidator<UpdateUserRequest> updateValidator,
        ILogger<GrpcUserService> logger,
        UserMapper mapper)
    {
        _userService = userService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
        _mapper = mapper;
    }

    public override async Task<UserId> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        _logger.LogInformation("CreateUser called. Login: {Login}, Name: {Name}, Surname: {Surname}, Age: {Age}",
            request.Login, request.Name, request.Surname, request.Age);

        await _createValidator.ValidateAndThrowAsync(request);
        try
        {
            var createdId = await _userService.Create(_mapper.ToCreateModel(request));

            return new UserId { Id = createdId };
        }
        catch (LoginConflictException exception)
        {
            throw new RpcException(new Status(
                StatusCode.AlreadyExists,
                exception.Message)
            );
        }
    }

    public override async Task<UserResponse> GetUserById(UserId request, ServerCallContext context)
    {
        _logger.LogInformation("GetUserById called. UserId: {Id}", request.Id);

        try
        {
            var model = await _userService.GetById(request.Id);

            return _mapper.FromModel(model);
        }
        catch (UserNotFoundException exception)
        {
            throw new RpcException(new Status(
                    StatusCode.NotFound,
                    exception.Message
                )
            );
        }
    }

    public override async Task GetUserByName(
        UserFullName request,
        IServerStreamWriter<UserResponse> responseStream,
        ServerCallContext context)
    {
        _logger.LogInformation("GetUserByName called. Name: {Name}, Surname: {Surname}", request.Name, request.Surname);

        try
        {
            var model = await _userService.GetByName(request.Name, request.Surname);

            await responseStream.WriteAsync(_mapper.FromModel(model));
        }
        catch (UserNotFoundException exception)
        {
            throw new RpcException(new Status(
                    StatusCode.NotFound,
                    exception.Message
                )
            );
        }
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
        try
        {
            // TODO: must throw UserNotFoundException
            var id = await _userService.Update(_mapper.ToUpdateModel(request));

            return new UserId { Id = id };
        }
        catch (UserNotFoundException exception)
        {
            throw new RpcException(new Status(
                    StatusCode.NotFound,
                    exception.Message
                )
            );
        }
    }

    public override async Task<UserId> DeleteUser(UserId request, ServerCallContext context)
    {
        _logger.LogInformation("DeleteUser called. UserId: {Id}", request.Id);

        try
        {
            var id = await _userService.Delete(request.Id);

            return new UserId { Id = id };
        }
        catch (UserNotFoundException exception)
        {
            throw new RpcException(new Status(
                    StatusCode.NotFound,
                    exception.Message
                )
            );
        }
    }
}