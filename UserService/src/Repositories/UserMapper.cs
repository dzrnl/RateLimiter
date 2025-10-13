using Riok.Mapperly.Abstractions;
using UserService.Repositories.Entities;
using UserService.Services.Models;

namespace UserService.Repositories;

[Mapper]
public partial class UserMapper
{
    public partial UserEntity FromModel(UserModel model);

    public partial UserModel ToModel(UserEntity entity);

    public partial IEnumerable<UserModel> ToModels(IEnumerable<UserEntity> entities);
}