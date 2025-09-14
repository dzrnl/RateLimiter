namespace UserService.Services;

public interface IUserService
{
    UserModel GetById(int id);

    UserModel GetByName(string name, string surname);

    IList<UserModel> GetAll();

    int Create(CreateUserDto dto);

    int Update(UpdateUserDto dto);

    void DeleteById(int id);
}