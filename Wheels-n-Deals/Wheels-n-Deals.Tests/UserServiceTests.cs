using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;
using Wheels_n_Deals.API.Services;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.Tests;

public class UserServiceTests
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService = Substitute.For<IAuthService>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    public UserServiceTests()
    {
        _userService = new UserService(_authService, _unitOfWork);
    }

    [Fact]
    public async Task GetUserAsyncById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userName = "Andrei Corbu";
        var userDb = new User
        {
            Id = userId,
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "andreicorbu7@gmail.com"
        };
        _unitOfWork.Users.GetUserAsync(userId).Returns(userDb);

        // Act
        var user = await _userService.GetUserAsync(userId);

        // Assert
        user.Should().NotBeNull();
        user.Id.Should().Be(userId);
        user.FullName.Should().Be(userName);
    }

    [Fact]
    public async Task GetUserAsyncById_ShouldThrow_WhenUserNotExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _unitOfWork.Users.GetUserAsync(userId).ReturnsNull();

        // Act
        var action = async () => await _userService.GetUserAsync(userId);

        // Assert
        await action.Should().ThrowAsync<ResourceMissingException>()
            .WithMessage($"User with id {userId} does not exist!");
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
        user.FullName.Should().Be(userName);
        user.Email.Should().Be(email);
    }

    [Fact]
    public async Task GetUserAsyncByEmail_ShouldThrow_WhenUserNotExists()
    {
        // Arrange
        var email = "andreicorbu7@gmail.com";
        _unitOfWork.Users.GetUserAsync(email).ReturnsNull();

        // Act
        var action = async () => await _userService.GetUserAsync(email);

        // Assert
        await action.Should().ThrowAsync<ResourceMissingException>()
            .WithMessage($"User with email {email} does not exist!");
    }
}