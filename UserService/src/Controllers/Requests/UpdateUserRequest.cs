using UserService.Services.Dtos;

namespace UserService;

public partial class UpdateUserRequest : IUpdateUserDto
{
    string? IUpdateUserDto.Password => HasPassword ? Password : null;
    
    string? IUpdateUserDto.Name => HasName ? Name : null;

    string? IUpdateUserDto.Surname => HasSurname ? Surname : null;

    int? IUpdateUserDto.Age => HasAge ? Age : null;
}