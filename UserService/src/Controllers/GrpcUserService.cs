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
    private readonly UserMapper _mapper;

    public GrpcUserService(
        IUserService userService,
        IValidator<CreateUserRequest> createValidator,
        IValidator<UpdateUserRequest> updateValidator,
        UserMapper mapper)
    {
        _userService = userService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }

    public override async Task<UserId> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        _createValidator.ValidateAndThrow(request);

        var createModel = _mapper.ToCreateModel(request);
        var user = await _userService.CreateUserAsync(createModel, context.CancellationToken);

        return new UserId { Id = user.Id };
    }

    public override async Task<UserResponse> GetUserById(UserId request, ServerCallContext context)
    {
        var user = await _userService.GetUserByIdAsync(request.Id, context.CancellationToken);
        return _mapper.FromModel(user);
    }

    public override async Task GetUsersByName(
        UserFullName request,
        IServerStreamWriter<UserResponse> responseStream,
        ServerCallContext context)
    {
        var users = await _userService.FindUsersByNameAsync(request.Name, request.Surname, context.CancellationToken);

        foreach (var user in users)
        {
            await responseStream.WriteAsync(_mapper.FromModel(user), context.CancellationToken);
        }
    }

    public override async Task<UserId> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        _updateValidator.ValidateAndThrow(request);

        var updateModel = _mapper.ToUpdateModel(request);
        var user = await _userService.UpdateUserAsync(updateModel, context.CancellationToken);

        return new UserId { Id = user.Id };
    }

    public override async Task<UserId> DeleteUser(UserId request, ServerCallContext context)
    {
        var userId = await _userService.DeleteUserAsync(request.Id, context.CancellationToken);
        return new UserId { Id = userId };
    }
}