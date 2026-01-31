using Riok.Mapperly.Abstractions;
using UserService.Services.Models;

namespace UserService.Controllers;

[Mapper]
public partial class UserMapper
{
    [MapperIgnoreSource(nameof(IUserModel.Password))]
    public partial UserResponse ToResponse(IUserModel model);
}