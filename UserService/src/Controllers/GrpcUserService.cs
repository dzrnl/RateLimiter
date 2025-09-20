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
        await _createValidator.ValidateAndThrowAsync(request);

        var user = await _userService.Create(_mapper.ToCreateModel(request));
        return new UserId { Id = user.Id };
    }

    public override async Task<UserResponse> GetUserById(UserId request, ServerCallContext context)
    {
        var user = await _userService.GetById(request.Id);
        return _mapper.FromModel(user);
    }

    public override async Task GetUserByName(
        UserFullName request,
        IServerStreamWriter<UserResponse> responseStream,
        ServerCallContext context)
    {
        var users = await _userService.GetByName(request.Name, request.Surname);

        foreach (var user in users)
        {
            await responseStream.WriteAsync(_mapper.FromModel(user));
        }
    }

    public override async Task<UserId> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var user = await _userService.Update(_mapper.ToUpdateModel(request));
        return new UserId { Id = user.Id };
    }

    public override async Task<UserId> DeleteUser(UserId request, ServerCallContext context)
    {
        var userId = await _userService.Delete(request.Id);
        return new UserId { Id = userId };
    }
}