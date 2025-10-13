using Riok.Mapperly.Abstractions;
using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Controllers;

[Mapper]
public partial class UserMapper
{
    [MapperIgnoreSource(nameof(UserModel.Password))]
    public partial UserResponse FromModel(UserModel model);

    public partial CreateUserDto ToCreateModel(CreateUserRequest request);

    public UpdateUserDto ToUpdateModel(UpdateUserRequest request)
    {
        return new UpdateUserDto(
            request.Id,
            request.HasPassword ? request.Password : null,
            request.HasName ? request.Name : null,
            request.HasSurname ? request.Surname : null,
            request.HasAge ? request.Age : null
        );
    }
}