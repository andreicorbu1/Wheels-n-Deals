using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;
using Wheels_n_Deals.API.Services;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.Tests.Services;

public class AnnouncementServiceTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IAnnouncementService _announcementService;

    public AnnouncementServiceTests()
    {
        _announcementService = new AnnouncementService(_unitOfWork);
    }

    [Fact]
    public async Task AddAnnouncementAsync_ShouldReturnGuid_WhenAnnouncementIsValid()
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
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = user,
            Feature = new(),
            Id = Guid.NewGuid(),
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Users.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.Vehicles.GetVehicleAsync(vehicle.VinNumber).Returns(vehicle);
        var addAnn = new AddAnnouncementDto
        {
            VinNumber = "SOMEVIN",
            City = "Targoviste",
            County = "Dambovita",
            Description = "Some Description",
            ImagesUrl = new(),
            Title = "Some title",
            UserId = user.Id
        };
        var annId = Guid.NewGuid();
        _unitOfWork.Announcements.AddAsync(Arg.Any<Announcement>()).Returns(new Announcement {  Id = annId  });

        // Act
        var res = await _announcementService.AddAnnouncementAsync(addAnn);

        // Assert
        res.Should().NotBe(Guid.Empty);
        res.Should().Be(annId);
    }

    [Fact]
    public async Task AddAnnouncementAsync_ShouldThrow_WhenVehicleNull()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = new(),
            Feature = new(),
            Id = Guid.NewGuid(),
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Vehicles.GetVehicleAsync(vehicle.VinNumber).ReturnsNull();
        var addAnn = new AddAnnouncementDto
        {
            VinNumber = "SOMEVIN",
            City = "Targoviste",
            County = "Dambovita",
            Description = "Some Description",
            ImagesUrl = new(),
            Title = "Some title",
            UserId = Guid.NewGuid()
        };

        // Act
        var action = async () => await _announcementService.AddAnnouncementAsync(addAnn);

        // Assert
        await action.Should().ThrowAsync<ResourceMissingException>();
    }

    [Fact]
    public async Task AddAnnouncementAsync_ShouldThrow_WhenUserNotAdmin()
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
            Address = "Romania",
            Role = Role.User
        };
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = new(),
            Feature = new(),
            Id = Guid.NewGuid(),
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Users.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.Vehicles.GetVehicleAsync(vehicle.VinNumber).Returns(vehicle);
        var addAnn = new AddAnnouncementDto
        {
            VinNumber = "SOMEVIN",
            City = "Targoviste",
            County = "Dambovita",
            Description = "Some Description",
            ImagesUrl = new(),
            Title = "Some title",
            UserId = user.Id
        };

        // Act
        var action = async () => await _announcementService.AddAnnouncementAsync(addAnn);

        // Assert
        await action.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task AddAnnouncementAsync_ShouldThrow_WhenAddAnnouncementNull()
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
            Address = "Romania",
            Role = Role.User
        };
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = new(),
            Feature = new(),
            Id = Guid.NewGuid(),
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Users.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.Vehicles.GetVehicleAsync(vehicle.VinNumber).Returns(vehicle);
        AddAnnouncementDto addAnn = null;

        // Act
        var action = async () => await _announcementService.AddAnnouncementAsync(addAnn);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAnnouncementAsync_ShouldReturnAnnouncement_WhenAnnouncementExists()
    {
        // Arrange
        var announcement = new Announcement { Id = Guid.NewGuid() };
        _unitOfWork.Announcements.DeleteAsync(announcement.Id).Returns(announcement);

        // Act
        var returned = await _announcementService.DeleteAnnouncementAsync(announcement.Id);

        // Assert
        returned.Should().NotBeNull();
        returned?.Id.Should().Be(announcement.Id);
    }

    [Fact]
    public async Task DeleteAnnouncementAsync_ShouldThrow_WhenAnnouncementDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Announcements.DeleteAsync(id).ReturnsNull();

        // Act
        var action = async () => await _announcementService.DeleteAnnouncementAsync(id);

        // Assert
        await action.Should().ThrowAsync<ResourceMissingException>()
            .WithMessage($"Announcement with id {id} does not exist");
    }

    [Fact]
    public async Task GetAnnouncementAsync_ShouldReturnAnnouncement_WhenAnnouncementExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ann = new Announcement { Id = id };
        _unitOfWork.Announcements.GetByIdAsync(id).Returns(ann);

        // Act
        var ret = await _announcementService.GetAnnouncementAsync(id);

        // Assert
        ret.Should().NotBeNull();
        ret.Should().Be(ann);
    }

    [Fact]
    public async Task GetAnnouncementAsync_ShouldReturnNull_WhenAnnouncementDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Announcements.GetByIdAsync(id).ReturnsNull();

        // Act
        var ret = await _announcementService.GetAnnouncementAsync(id);

        // Assert
        ret.Should().BeNull();
    }

    [Fact]
    public async Task GetAnnouncementsAsync_ShouldReturnList_WhenAnnouncementsExist()
    {
        // Arrange
        var ann = new List<Announcement> { new Announcement { } };
        _unitOfWork.Announcements.GetManyAsync(Arg.Any<Expression<Func<Announcement, bool>>>(), Arg.Any<Func<IQueryable<Announcement>, IOrderedQueryable<Announcement>>>()).Returns(ann);

        // Act
        var ret = await _announcementService.GetAnnouncementsAsync(null);

        // Assert
        ret.Should().NotBeNull();
        ret.Should().NotBeEmpty();
        ret.First().Should().Be(ann.First());
    }

    [Fact]
    public async Task GetAnnouncementsAsync_ShouldReturnEmptyList_WhenAnnouncementsDoNotExist()
    {
        // Arrange
        _unitOfWork.Announcements.GetManyAsync(Arg.Any<Expression<Func<Announcement, bool>>>(), Arg.Any<Func<IQueryable<Announcement>, IOrderedQueryable<Announcement>>>()).Returns(new List<Announcement>());

        // Act
        var ret = await _announcementService.GetAnnouncementsAsync(null);

        // Assert
        ret.Should().NotBeNull();
        ret.Should().BeEmpty();
    }

    [Fact]
    public async Task RenwAnnouncementAsync_ShouldReturnAnnouncement_WhenAnnouncementsExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ann = new Announcement
        {
            Id = id,
            DateModified = DateTime.UtcNow.AddDays(-1)
        };
        _unitOfWork.Announcements.UpdateAsync(ann).Returns(ann);
        _unitOfWork.Announcements.GetByIdAsync(id).Returns(ann);

        // Act
        var ret = await _announcementService.RenewAnnouncementAsync(id);

        // Assert
        ret.Should().NotBeNull();
        ret.Should().Be(ann);
    }

    [Fact]
    public async Task RenwAnnouncementAsync_ShouldThrow_WhenAnnouncementsCannotBeRenewed()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ann = new Announcement
        {
            Id = id,
            DateModified = DateTime.UtcNow
        };
        _unitOfWork.Announcements.UpdateAsync(ann).Returns(ann);
        _unitOfWork.Announcements.GetByIdAsync(id).Returns(ann);

        // Act
        var ret = async () => await _announcementService.RenewAnnouncementAsync(id);

        // Assert
        await ret.Should().ThrowAsync<RenewTimeNotElapsedException>();
    }

    [Fact]
    public async Task RenwAnnouncementAsync_ShouldThrow_WhenAnnouncementsDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Announcements.GetByIdAsync(id).ReturnsNull();

        // Act
        var ret = async () => await _announcementService.RenewAnnouncementAsync(id);

        // Assert
        await ret.Should().ThrowAsync<ResourceMissingException>();
    }

    [Fact]
    public async Task UpdateAnnouncementAsync_ShouldReturnAnnouncement_WhenAnnouncementValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ann = new Announcement
        {
            Id = id,
            DateModified = DateTime.UtcNow
        };
        var upd = new UpdateAnnouncementDto
        {
            City = "Targoviste",
            County = "Dambovita",
            Description = "Vand skoda",
            ImagesUrl = new List<ImageDto>
            {
                new ImageDto("string1"),
                new ImageDto("string2"),
                new ImageDto("string3")
            },
            Title = "Vand skoda",
            VinNumber = "SOMEVIN"
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "andreicorbu7@gmail.com",
            PhoneNumber = "0733888999",
            HashedPassword = "SomePass",
            Address = "Romania",
            Role = Role.User
        };
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = user,
            Feature = new(),
            Id = Guid.NewGuid(),
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Vehicles.GetVehicleAsync(vehicle.VinNumber).Returns(vehicle);
        _unitOfWork.Vehicles.GetByIdAsync(vehicle.Id).Returns(vehicle);
        _unitOfWork.Users.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.Announcements.GetByIdAsync(id).Returns(ann);
        _unitOfWork.Announcements.UpdateAsync(ann).Returns(ann);

        // Act
        var ret = await _announcementService.UpdateAnnouncementAsync(id, upd);

        // Assert
        ret.Should().NotBeNull();
        ret?.Vehicle?.VinNumber.Should().Be(vehicle.VinNumber);
        ret?.Owner?.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task UpdateAnnouncementAsync_ShouldThrow_WhenVehicleDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ann = new Announcement
        {
            Id = id,
            DateModified = DateTime.UtcNow
        };
        var upd = new UpdateAnnouncementDto
        {
            City = "Targoviste",
            County = "Dambovita",
            Description = "Vand skoda",
            ImagesUrl = new List<ImageDto>
            {
                new ImageDto("string1"),
                new ImageDto("string2"),
                new ImageDto("string3")
            },
            Title = "Vand skoda",
            VinNumber = "SOMEVIN"
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "andreicorbu7@gmail.com",
            PhoneNumber = "0733888999",
            HashedPassword = "SomePass",
            Address = "Romania",
            Role = Role.User
        };
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = user,
            Feature = new(),
            Id = Guid.NewGuid(),
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Vehicles.GetVehicleAsync(vehicle.VinNumber).ReturnsNull();
        _unitOfWork.Vehicles.GetByIdAsync(vehicle.Id).ReturnsNull();
        _unitOfWork.Users.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.Announcements.GetByIdAsync(id).Returns(ann);
        _unitOfWork.Announcements.UpdateAsync(ann).Returns(ann);

        // Act
        var ret = async () => await _announcementService.UpdateAnnouncementAsync(id, upd);

        // Assert
        await ret.Should().ThrowAsync<ResourceMissingException>();
    }

    [Fact]
    public async Task UpdateAnnouncementAsync_ShouldThrow_WhenUpdateAnnouncementNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ann = new Announcement
        {
            Id = id,
            DateModified = DateTime.UtcNow
        };
        var upd = new UpdateAnnouncementDto
        {
            City = "Targoviste",
            County = "Dambovita",
            Description = "Vand skoda",
            ImagesUrl = new List<ImageDto>
            {
                new ImageDto("string1"),
                new ImageDto("string2"),
                new ImageDto("string3")
            },
            Title = "Vand skoda",
            VinNumber = "SOMEVIN"
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "andreicorbu7@gmail.com",
            PhoneNumber = "0733888999",
            HashedPassword = "SomePass",
            Address = "Romania",
            Role = Role.User
        };
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = user,
            Feature = new(),
            Id = Guid.NewGuid(),
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Vehicles.GetVehicleAsync(vehicle.VinNumber).ReturnsNull();
        _unitOfWork.Vehicles.GetByIdAsync(vehicle.Id).ReturnsNull();
        _unitOfWork.Users.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.Announcements.GetByIdAsync(id).Returns(ann);
        _unitOfWork.Announcements.UpdateAsync(ann).Returns(ann);

        // Act
        var ret = async () => await _announcementService.UpdateAnnouncementAsync(id, null);

        // Assert
        await ret.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAnnouncementAsync_ShouldThrow_WhenAnnouncementDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ann = new Announcement
        {
            Id = Guid.NewGuid(),
            DateModified = DateTime.UtcNow
        };
        var upd = new UpdateAnnouncementDto
        {
            City = "Targoviste",
            County = "Dambovita",
            Description = "Vand skoda",
            ImagesUrl = new List<ImageDto>
            {
                new ImageDto("string1"),
                new ImageDto("string2"),
                new ImageDto("string3")
            },
            Title = "Vand skoda",
            VinNumber = "SOMEVIN"
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Andrei",
            LastName = "Corbu",
            Email = "andreicorbu7@gmail.com",
            PhoneNumber = "0733888999",
            HashedPassword = "SomePass",
            Address = "Romania",
            Role = Role.User
        };
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = user,
            Feature = new(),
            Id = Guid.NewGuid(),
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Vehicles.GetVehicleAsync(vehicle.VinNumber).ReturnsNull();
        _unitOfWork.Vehicles.GetByIdAsync(vehicle.Id).ReturnsNull();
        _unitOfWork.Users.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.Announcements.GetByIdAsync(id).ReturnsNull();
        _unitOfWork.Announcements.UpdateAsync(ann).Returns(ann);

        // Act
        var ret = async () => await _announcementService.UpdateAnnouncementAsync(id, null);

        // Assert
        await ret.Should().ThrowAsync<ResourceMissingException>();
    }
}