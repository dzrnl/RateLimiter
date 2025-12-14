using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using UserService.Repositories;
using UserService.Services;
using UserService.Services.Dtos;
using UserService.Services.Models;
using Xunit;
using DomainUserService = UserService.Services.UserService;

namespace UserService.tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly DomainUserService _userService;
    private readonly IFixture _fixture;

    public UserServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _userService = new DomainUserService(_repositoryMock.Object);
        _fixture = new Fixture()
            .Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true
            });
    }

    [Fact]
    public async Task CreateUser_WhenLoginExists_ThrowsLoginConflictException()
    {
        // Arrange
        var dto = _fixture.Create<ICreateUserDto>();

        _repositoryMock.Setup(r => r.FindByLoginAsync(dto.Login, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateUserModel(1, dto.Login, dto.Password, dto.Name, dto.Surname, dto.Age));

        // Act & Assert
        await Assert.ThrowsAsync<LoginConflictException>(() => _userService.CreateUserAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateUser_WhenLoginDoesNotExist_CreatesUser()
    {
        // Arrange
        var dto = _fixture.Create<ICreateUserDto>();

        _repositoryMock.Setup(r => r.FindByLoginAsync(dto.Login, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IUserModel?)null);
        _repositoryMock.Setup(r => r.AddAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateUserModel(1, dto.Login, dto.Password, dto.Name, dto.Surname, dto.Age));

        // Act
        var result = await _userService.CreateUserAsync(dto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Login, result.Login);
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var user = _fixture.Create<IUserModel>();
        _repositoryMock.Setup(r => r.FindByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(user.Id, CancellationToken.None);

        // Assert
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Login, result.Login);
    }

    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.FindByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IUserModel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserByIdAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task FindUsersByName_WhenUsersExist_ReturnsUsers()
    {
        // Arrange
        var name = _fixture.Create<string>();
        var surname = _fixture.Create<string>();
        var users = Enumerable.Range(0, 3)
            .Select(_ =>
            {
                var mock = new Mock<IUserModel>();
                mock.SetupGet(x => x.Name).Returns(name);
                mock.SetupGet(x => x.Surname).Returns(surname);
                return mock.Object;
            })
            .ToArray();

        _repositoryMock.Setup(r => r.FindAllByNameAsync(name, surname, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _userService.FindUsersByNameAsync(name, surname, CancellationToken.None);

        // Assert
        Assert.Equal(users.Length, result.Length);
        Assert.All(result, u => {
            Assert.Equal(name, u.Name);
            Assert.Equal(surname, u.Surname);
        });
    }

    [Fact]
    public async Task UpdateUser_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        var dtoMock = new Mock<IUpdateUserDto>();
        dtoMock.SetupGet(x => x.Id).Returns(_fixture.Create<int>());
        dtoMock.SetupGet(x => x.Password).Returns(_fixture.Create<string>());

        var dto = dtoMock.Object;
        _repositoryMock.Setup(r => r.UpdateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IUserModel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateUserAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteUser_WhenUserExists_ReturnsUserId()
    {
        // Arrange
        var userId = _fixture.Create<int>();
        _repositoryMock.Setup(r => r.DeleteAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userId);

        // Act
        var result = await _userService.DeleteUserAsync(userId, CancellationToken.None);

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public async Task DeleteUser_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteUserAsync(1, CancellationToken.None));
    }

    private IUserModel CreateUserModel(int id, string login, string password, string name, string surname, int age)
    {
        var mock = _fixture.Create<Mock<IUserModel>>();

        mock.SetupGet(x => x.Id).Returns(id);
        mock.SetupGet(x => x.Login).Returns(login);
        mock.SetupGet(x => x.Password).Returns(password);
        mock.SetupGet(x => x.Name).Returns(name);
        mock.SetupGet(x => x.Surname).Returns(surname);
        mock.SetupGet(x => x.Age).Returns(age);

        return mock.Object;
    }
}