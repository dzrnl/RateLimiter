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
    public async Task Create_WhenLoginExists_ThrowsLoginConflictException()
    {
        // Arrange
        var dto = _fixture.Create<CreateUserDto>();

        _repositoryMock.Setup(r => r.GetUserByLoginAsync(dto.Login, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserModel(1, dto.Login, dto.Password, dto.Name, dto.Surname, dto.Age));

        // Act & Assert
        await Assert.ThrowsAsync<LoginConflictException>(() => _userService.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task Create_WhenLoginDoesNotExist_CreatesUser()
    {
        // Arrange
        var dto = _fixture.Create<CreateUserDto>();

        _repositoryMock.Setup(r => r.GetUserByLoginAsync(dto.Login, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserModel?)null);
        _repositoryMock.Setup(r => r.CreateUserAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserModel(1, dto.Login, dto.Password, dto.Name, dto.Surname, dto.Age));

        // Act
        var result = await _userService.CreateAsync(dto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Login, result.Login);
    }

    [Fact]
    public async Task GetById_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var user = _fixture.Create<UserModel>();
        _repositoryMock.Setup(r => r.GetUserByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetByIdAsync(user.Id, CancellationToken.None);

        // Assert
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Login, result.Login);
    }

    [Fact]
    public async Task GetById_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserModel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetByIdAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task Update_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        var dto = _fixture.Build<UpdateUserDto>()
            .With(x => x.Password, _fixture.Create<string>())
            .Create();
        _repositoryMock.Setup(r => r.UpdateUserAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserModel?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.UpdateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task Delete_WhenUserExists_ReturnsUserId()
    {
        // Arrange
        var userId = _fixture.Create<int>();
        _repositoryMock.Setup(r => r.DeleteUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userId);

        // Act
        var result = await _userService.DeleteAsync(userId, CancellationToken.None);

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public async Task Delete_WhenUserDoesNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteUserAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteAsync(1, CancellationToken.None));
    }
}