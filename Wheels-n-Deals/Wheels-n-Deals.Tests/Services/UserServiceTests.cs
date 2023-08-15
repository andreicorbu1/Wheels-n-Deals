using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;
using Wheels_n_Deals.API.Services;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.Tests;

public class UserServiceTests
{
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userService = new UserService(_authService, _unitOfWork);
    }

    [Fact]
    public async Task GetUserAsyncById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDb = new User
        {
            Id = userId,
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "andreicorbu7@gmail.com",
            PhoneNumber = "0733888999",
            HashedPassword = "SomePass",
            Address = "Romania"
        };
        _unitOfWork.Users.GetUserAsync(userId).Returns(userDb);

        // Act
        var user = await _userService.GetUserAsync(userId);

        // Assert
        user.Should().NotBeNull();
        user?.Id.Should().Be(userId);
        user?.FullName.Should().Be("Andrei Corbu");
    }

    [Fact]
    public async Task GetUserAsyncById_ShouldReturnNull_WhenUserNotExists()
    {
        // Arrange
        _unitOfWork.Users.GetUserAsync(Arg.Any<Guid>()).ReturnsNull();
        var userId = Guid.NewGuid();

        // Act
        var user = await _userService.GetUserAsync(userId);

        // Assert
        user.Should().BeNull();
    }

    [Fact]
    public async Task GetUserAsyncByEmail_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var email = "andreicorbu7@gmail.com";
        var userName = "Andrei Corbu";
        var userDb = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = email
        };
        _unitOfWork.Users.GetUserAsync(email).Returns(userDb);

        // Act
        var user = await _userService.GetUserAsync(email);

        // Assert
        user.Should().NotBeNull();
        user?.FullName.Should().Be(userName);
        user?.Email.Should().Be(email);
    }

    [Fact]
    public async Task GetUserAsyncByEmail_ShouldReturnNull_WhenUserNotExist()
    {
        // Arrange
        var email = "andreicorbu7@gmail.com";
        _unitOfWork.Users.GetUserAsync(Arg.Any<string>()).ReturnsNull();

        // Act
        var user = await _userService.GetUserAsync(email);

        // Assert
        user.Should().BeNull();
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "andreicorbu7@gmail.com",
            PhoneNumber = "0733888999",
            HashedPassword = "SomePass",
            Address = "Romania"
        };
        _unitOfWork.Users.GetUserAsync(userId).Returns(user);
        _unitOfWork.Users.RemoveAsync(userId).Returns(user);

        // Act
        var res = await _userService.DeleteUserAsync(userId);

        // Assert
        res.Should().NotBeNull();
        res?.FullName.Should().Be("Andrei Corbu");
        res?.Id.Should().Be(userId);
        res?.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldThrow_WhenUserNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Users.GetUserAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var action = async () => await _userService.DeleteUserAsync(id);

        // Assert
        await action.Should().ThrowAsync<ResourceMissingException>()
            .WithMessage($"User with id {id} does not exist!");
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnList_WhenUsersExist()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "andreicorbu7@gmail.com",
            PhoneNumber = "0733888999",
            HashedPassword = "SomePass",
            Address = "Romania"
        };
        var list = new List<User> { user };
        _unitOfWork.Users.GetUsersAsync().Returns(list);

        // Act
        var users = await _userService.GetUsersAsync();

        // Assert
        users.Should().NotBeNull();
        users.Should().HaveCount(1);
        users.Should().Contain(user);
    }

    [Fact]
    public async Task GetUsersAsync_ShouldReturnEmptyList_WhenUsersNotExist()
    {
        // Arrange
        _unitOfWork.Users.GetUsersAsync().Returns(new List<User>());

        // Act
        var users = await _userService.GetUsersAsync();

        // Assert
        users.Should().NotBeNull();
        users.Should().BeEmpty();
    }

    [Fact]
    public async Task LoginUserAsync_ShouldReturnJWT_WhenUsersExists()
    {
        // Arrange
        var login = new LoginDto("test", "test");
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "test",
            PhoneNumber = "0733888999",
            HashedPassword = "test",
            Address = "Romania"
        };
        _unitOfWork.Users.GetUserAsync(login.Email).Returns(user);
        _authService.VerifyHashedPassword(user.HashedPassword, login.Password).Returns(true);
        _authService.GetToken(user).Returns(
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiQWRtaW4iLCJuYW1laWQiOiIwMmQ2MjlhZC0yNjg4LTRmY2EtYTQ2Mi02ZTQ1MWZkZWM2YTkiLCJlbWFpbCI6InRlc3QiLCJuYmYiOjE2OTE4MzQ5NzgsImV4cCI6MTY5MTgzODU3OCwiaWF0IjoxNjkxODM0OTc4LCJpc3MiOiJCYWNrZW5kIiwiYXVkIjoiRnJvbnRlbmQifQ.Gzj4FplIUyN3xG_HAT07RZo5SRyGPuFtaBA12Zsp_oE");

        // Act
        var token = await _userService.LoginUserAsync(login);

        // Assert
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoginUserAsync_ShouldReturnEmptyString_WhenPasswordWrong()
    {
        // Arrange
        var login = new LoginDto("test", "test");
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "test",
            PhoneNumber = "0733888999",
            HashedPassword = "test",
            Address = "Romania"
        };
        _unitOfWork.Users.GetUserAsync(login.Email).Returns(user);
        _authService.VerifyHashedPassword(user.HashedPassword, login.Password).Returns(false);
        _authService.GetToken(user).Returns(string.Empty);

        // Act
        var token = await _userService.LoginUserAsync(login);

        // Assert
        token.Should().BeEmpty();
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnNewGuid_WhenUserWithSameEmailDoesNotExist()
    {
        // Arrange
        var registerDto = new RegisterDto("test", "test", "Andrei", "Corbu", "0733888999", "Romania");
        _unitOfWork.Users.Any(Arg.Any<Func<User, bool>>()).Returns(false);
        var id = Guid.NewGuid();
        _unitOfWork.Users.InsertAsync(Arg.Any<User>()).Returns(id);

        // Act
        var actualId = await _userService.RegisterUserAsync(registerDto);

        // Assert
        actualId.Should().Be(id);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrow_WhenUserWithSameEmailDoesExist()
    {
        // Arrange
        var registerDto = new RegisterDto("test", "test", "Andrei", "Corbu", "0733888999", "Romania");
        _unitOfWork.Users.Any(Arg.Any<Func<User, bool>>()).Returns(true);

        // Act
        var action = async () => await _userService.RegisterUserAsync(registerDto);

        // Assert
        await action.Should().ThrowAsync<ResourceExistingException>()
            .WithMessage($"User with email '{registerDto.Email}' already exists!");
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrow_WhenDtoIsEmpty()
    {
        // Arrange
        RegisterDto registerDto = null;

        // Act
        var action = async () => await _userService.RegisterUserAsync(registerDto);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldReturnUpdatedUser_WhenValidUser()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateUserDto = new UpdateUserDto
        {
            Id = id,
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "test",
            PhoneNumber = "0733888999",
            Password = "test",
            Address = "Romania"
        };
        var user = new User
        {
            Id = id,
            FirstName = "Test1",
            LastName = "Test2",
            Email = "test",
            PhoneNumber = "0733888999",
            HashedPassword = "test",
            Address = "Romania"
        };
        _unitOfWork.Users.GetUserAsync(id).Returns(user);
        _unitOfWork.Users.UpdateAsync(Arg.Any<User>()).Returns(user);
        _authService.HashPassword(Arg.Any<string>()).Returns(updateUserDto.PhoneNumber);

        // Act
        var result = await _userService.UpdateUserAsync(updateUserDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(user);
        result?.FullName.Should().Be(user.FullName);
        result?.Address.Should().Be(user.Address);
        result?.HashedPassword.Should().Be(user.HashedPassword);
        result?.Email.Should().Be(user.Email);
        result?.PhoneNumber.Should().Be(user.PhoneNumber);
        result?.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldThrow_WhenUserNotFound()
    {
        // Arrange
        var updateUserDto = new UpdateUserDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "test",
            PhoneNumber = "0733888999",
            Password = "test",
            Address = "Romania"
        };
        _unitOfWork.Users.GetUserAsync(updateUserDto.Id).ReturnsNull();

        // Act
        var action = async () => await _userService.UpdateUserAsync(updateUserDto);

        // Assert
        await action.Should().ThrowAsync<ResourceMissingException>()
            .WithMessage($"User with id {updateUserDto.Id} does not exists!");
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldThrow_WhenDtoNotFound()
    {
        // Arrange
        UpdateUserDto dto = null;
        var dtoWithEmptyEmail = new UpdateUserDto
        {
            Email = ""
        };

        // Act
        var action = async () => await _userService.UpdateUserAsync(dto);
        var action2 = async () => await _userService.UpdateUserAsync(dtoWithEmptyEmail);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
        await action2.Should().ThrowAsync<ArgumentNullException>();
    }
}