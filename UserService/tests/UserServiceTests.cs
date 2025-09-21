using AutoFixture;
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
    private readonly Fixture _fixture;

    public UserServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _userService = new DomainUserService(_repositoryMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CreateUser_WhenLoginExists_ThrowsLoginConflictException()
    {
        // Arrange
        var dto = _fixture.Create<CreateUserDto>();

        _repositoryMock.Setup(r => r.GetByLoginAsync(dto.Login, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserModel(1, dto.Login, dto.Password, dto.Name, dto.Surname, dto.Age));

        // Act & Assert
        await Assert.ThrowsAsync<LoginConflictException>(() => _userService.CreateUserAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateUser_WhenLoginDoesNotExist_CreatesUser()
    {
        // Arrange
        var dto = _fixture.Create<CreateUserDto>();

        _repositoryMock.Setup(r => r.GetByLoginAsync(dto.Login, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserModel?)null);
        _repositoryMock.Setup(r => r.AddAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserModel(1, dto.Login, dto.Password, dto.Name, dto.Surname, dto.Age));

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
        var user = _fixture.Create<UserModel>();
        _repositoryMock.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
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
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserModel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserByIdAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateUser_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        var dto = _fixture.Build<UpdateUserDto>()
            .With(x => x.Password, _fixture.Create<string>())
            .Create();
        _repositoryMock.Setup(r => r.UpdateAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserModel?)null);

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
}