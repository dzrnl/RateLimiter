using Riok.Mapperly.Abstractions;
using UserService.Services.Dtos;
using UserService.Services.Models;

namespace UserService.Controllers;

[Mapper]
public partial class UserMapper
{
    [MapperIgnoreSource(nameof(userDomain.Password))]
    public partial UserResponse FromModel(UserModel userDomain);

    public partial CreateUserDto ToCreateModel(CreateUserRequest request);

    public partial UpdateUserDto ToUpdateModel(UpdateUserRequest request); // TODO
}