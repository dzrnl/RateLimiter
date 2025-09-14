using UserService.Repositories;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    async UserModel GetById(int id)
    {
        UserModel? user = await _userRepository.GetUserByIdAsync(id);
        if (user is null)
        {
            throw UserNotFoundException("Error: no user with such ID");
        }

        return user;
    }

    UserModel GetByName(string name, string surname)
    {
        UserModel? user = await _userRepository.GetUserByNameAsync(name, surname);
        if (user is null)
        {
            throw new UserNotFoundException("Error: no user with such name");
        }

        return user;
    }

    IList<UserModel> GetAll()
    {
        return _userRepository.GetAllUsersAsync();
    }

    int Create(CreateUserDto dto)
    {
        if (_userRepository.GetUserByLoginAsync(dto.Id) != null)
        {
            throw new LoginConflictException("Error: user with this login already exists");
        }

        return _userRepository.CreateUserAsync(dto);    
    }

    void Update(UpdateUserDto dto)
    {
        _userRepository.UpdateUserAsync(dto);
    }

    int DeleteById(int id)
    {
        int? id = _userRepository.DeleteById(id);
        if (id is null)
        {
            throw new UserNotFoundException("Error: no user with such ID");
        }

        return id;
    }
}