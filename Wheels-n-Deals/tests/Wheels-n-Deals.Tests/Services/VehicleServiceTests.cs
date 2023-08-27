using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using Wheels_n_Deals.API;
using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;
using Wheels_n_Deals.API.Services;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.Tests.Services;

public class VehicleServiceTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IVehicleService _vehicleService;
    
    public VehicleServiceTests()
    {
        _vehicleService = new VehicleService(_unitOfWork);
    }

    [Fact]
    public async Task AddVehicleAsync_ShouldReturnGuid_WhenValidVehicle()
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
        var vehId = Guid.NewGuid();
        var featId = Guid.NewGuid();
        var addVehicleDto = new AddVehicleDto
        {
            OwnerId = user.Id,
            CarBody = "Hatchback",
            EngineSize = 1395,
            FuelType = "Petrol",
            Gearbox = "Manual",
            HorsePower = 75,
            Make = "Skoda",
            Mileage = 190000,
            Model = "Fabia",
            PriceInEuro = 3000,
            TechnicalState = "Used",
            VinNumber = "SOMEVIN",
            Year = 2001
        };
        _unitOfWork.Features.GetFeatureAsync(Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<Gearbox>(), Arg.Any<Fuel>()).ReturnsNull();
        _unitOfWork.Features.AddAsync(Arg.Any<Feature>()).Returns(new Feature { Id = featId });
        _unitOfWork.Vehicles.AddAsync(Arg.Any<Vehicle>()).Returns(new Vehicle { Id = vehId });
        _unitOfWork.Vehicles.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();
        _unitOfWork.Users.GetByIdAsync(addVehicleDto.OwnerId).Returns(user);

        // Act
        var retId = await _vehicleService.AddVehicleAsync(addVehicleDto);

        // Assert
        retId.Should().Be(vehId);
    }

    [Fact]
    public async Task AddVehicleAsync_ShouldReturnEmptyGuid_WhenUserDoesNotExist()
    {
        // Arrange
        var vehId = Guid.NewGuid();
        var featId = Guid.NewGuid();
        var addVehicleDto = new AddVehicleDto
        {
            OwnerId = Guid.NewGuid(),
            CarBody = "Hatchback",
            EngineSize = 1395,
            FuelType = "Petrol",
            Gearbox = "Manual",
            HorsePower = 75,
            Make = "Skoda",
            Mileage = 190000,
            Model = "Fabia",
            PriceInEuro = 3000,
            TechnicalState = "Used",
            VinNumber = "SOMEVIN",
            Year = 2001
        };
        _unitOfWork.Features.GetFeatureAsync(Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<Gearbox>(), Arg.Any<Fuel>()).ReturnsNull();
        _unitOfWork.Features.AddAsync(Arg.Any<Feature>()).Returns(new Feature { Id = featId });
        _unitOfWork.Vehicles.AddAsync(Arg.Any<Vehicle>()).Returns(new Vehicle { Id = vehId });
        _unitOfWork.Vehicles.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();
        _unitOfWork.Users.GetByIdAsync(addVehicleDto.OwnerId).ReturnsNull();

        // Act
        var retId = await _vehicleService.AddVehicleAsync(addVehicleDto);

        // Assert
        retId.Should().Be(Guid.Empty);
    }

    [Fact]
    public async Task AddVehicleAsync_ShouldThrow_WhenVehicleWithSameVinExists()
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
        var vehId = Guid.NewGuid();
        var featId = Guid.NewGuid();
        var addVehicleDto = new AddVehicleDto
        {
            OwnerId = user.Id,
            CarBody = "Hatchback",
            EngineSize = 1395,
            FuelType = "Petrol",
            Gearbox = "Manual",
            HorsePower = 75,
            Make = "Skoda",
            Mileage = 190000,
            Model = "Fabia",
            PriceInEuro = 3000,
            TechnicalState = "Used",
            VinNumber = "SOMEVIN",
            Year = 2001
        };
        _unitOfWork.Features.GetFeatureAsync(Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<Gearbox>(), Arg.Any<Fuel>()).ReturnsNull();
        _unitOfWork.Features.AddAsync(Arg.Any<Feature>()).Returns(new Feature { Id = featId });
        _unitOfWork.Vehicles.AddAsync(Arg.Any<Vehicle>()).Returns(new Vehicle { Id = vehId });
        _unitOfWork.Vehicles.GetVehicleAsync(addVehicleDto.VinNumber).Returns(new Vehicle());
        _unitOfWork.Users.GetByIdAsync(addVehicleDto.OwnerId).Returns(user);

        // Act
        var action = async () => await _vehicleService.AddVehicleAsync(addVehicleDto);

        // Assert
        await action.Should().ThrowAsync<ResourceExistingException>($"A vehicle with the VIN {addVehicleDto.VinNumber} already exists!");
    }

    [Fact]
    public async Task DeleteVehicleAsync_ShouldReturnVehicle_WhenVehicleExists()
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
        _unitOfWork.Vehicles.RemoveAsync(Arg.Any<string>()).Returns(vehicle);

        // Act
        var ret = await _vehicleService.DeleteVehicleAsync(vehicle.VinNumber);

        // Assert
        ret.Should().NotBeNull();
        ret.Should().Be(vehicle);
    }

    [Fact]
    public async Task DeleteVehicleAsync_ShouldThrow_WhenVehicleDoesNotExist()
    {
        // Arrange
        _unitOfWork.Vehicles.RemoveAsync(Arg.Any<string>()).ReturnsNull();

        // Act
        var action = async () => await _vehicleService.DeleteVehicleAsync("SOMEVIN");

        // Assert
        await action.Should().ThrowAsync< ResourceMissingException>()
            .WithMessage("Vehicle with VIN SOMEVIN does not exist!");
    }

    [Fact]
    public async Task GetVehicleAsync_ShouldReturnVehicle_WhenVehicleExists()
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
        _unitOfWork.Vehicles.GetByIdAsync(vehicle.Id).Returns(vehicle);
        _unitOfWork.Vehicles.GetVehicleAsync(vehicle.VinNumber).Returns(vehicle);

        // Act
        var getById = await _vehicleService.GetVehicleAsync(vehicle.Id);
        var getByVin = await _vehicleService.GetVehicleAsync(vehicle.VinNumber);

        // Assert
        getById.Should().NotBeNull();
        getByVin.Should().NotBeNull();
        getById.Should().Be(vehicle);
        getByVin.Should().Be(vehicle);
    }

    [Fact]
    public async Task GetVehicleAsync_ShouldReturnNull_WhenVehicleDoesNotExist()
    {
        // Arrange
        _unitOfWork.Vehicles.GetVehicleAsync(Arg.Any<string>()).ReturnsNull();
        _unitOfWork.Vehicles.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var getById = await _vehicleService.GetVehicleAsync(Guid.NewGuid());
        var getByVin = await _vehicleService.GetVehicleAsync(string.Empty);

        // Assert
        getById.Should().BeNull();
        getByVin.Should().BeNull();
    }

    [Fact]
    public async Task GetVehiclesAsync_ShouldReturnList_WhenVehiclesExist()
    {
        // Arrange
        var vehicles = new List<Vehicle>()
        {
        new Vehicle
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
            }
        };
        _unitOfWork.Vehicles.GetManyAsync(Arg.Any<Expression<Func<Vehicle, bool>>>())
            .Returns(vehicles);

        // Act
        var ret = await _vehicleService.GetVehiclesAsync(null);

        // Assert
        ret.Should().NotBeNull();
        ret.Should().HaveCount(1);
        ret.Should().Contain(vehicles[0]);
    }

    [Fact]
    public async Task GetVehiclesAsync_ShouldReturnEmptyList_WhenVehiclesDoNotExist()
    {
        // Arrange
        var vehicles = new List<Vehicle>();
        _unitOfWork.Vehicles.GetManyAsync(Arg.Any<Expression<Func<Vehicle, bool>>>()).Returns(vehicles);

        // Act
        var ret = await _vehicleService.GetVehiclesAsync(null);

        // Assert
        ret.Should().NotBeNull();
        ret.Should().HaveCount(0);
    }

    [Fact]
    public async Task UpdateVehicleAsync_ShouldReturnUpdatedVehicle_WhenVehicleExists()
    {
        // Arrange
        var vehId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = new(),
            Feature = null,
            Id = vehId,
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Vehicles.GetByIdAsync(Arg.Any<Guid>()).Returns(vehicle);
        _unitOfWork.Vehicles.GetVehicleAsync("SOMEVIN2").ReturnsNull();
        var upd = new UpdateVehicleDto("SOMEVIN2", "Skoda", "Model", 2001, 190000, "Used", 3000, "Hatchback", "Petrol", 1395, "Manual", 75);
        _unitOfWork.Features.GetFeatureAsync(Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<Gearbox>(), Arg.Any<Fuel>()).ReturnsNull();
        
        // Act
        var veh = await _vehicleService.UpdateVehicleAsync(vehId, upd);

        // Assert
        veh.Should().NotBeNull();
        veh?.VinNumber.Should().Be("SOMEVIN2");
    }

    [Fact]
    public async Task UpdateVehicleAsync_ShouldThrow_WhenVehicleHasSameVin()
    {
        // Arrange
        var vehId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = new(),
            Feature = new(),
            Id = vehId,
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        var upd = new UpdateVehicleDto("SOMEVIN2", "Skoda", "Model", 2001, 190000, "Used", 3000, "Hatchback", "Petrol", 1395, "Manual", 75);
        _unitOfWork.Vehicles.GetByIdAsync(Arg.Any<Guid>()).Returns(vehicle);
        _unitOfWork.Vehicles.GetVehicleAsync("SOMEVIN2").Returns(new Vehicle());

        // Act
        var action = async () => await _vehicleService.UpdateVehicleAsync(vehId, upd);

        // Assert
        await action.Should().ThrowAsync<ResourceExistingException>()
            .WithMessage("Vehicle with VIN SOMEVIN2 already exists!");
    }

    [Fact]
    public async Task UpdateVehicleAsync_ShouldThrow_WhenVehicleDoesNotExist()
    {
        // Arrange
        var vehId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = new(),
            Feature = new(),
            Id = vehId,
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        var upd = new UpdateVehicleDto("SOMEVIN2", "Skoda", "Model", 2001, 190000, "Used", 3000, "Hatchback", "Petrol", 1395, "Manual", 75);
        _unitOfWork.Vehicles.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();
        _unitOfWork.Vehicles.GetVehicleAsync("SOMEVIN2").Returns(new Vehicle());

        // Act
        var action = async () => await _vehicleService.UpdateVehicleAsync(vehId, upd);

        // Assert
        await action.Should().ThrowAsync<ResourceMissingException>()
            .WithMessage($"Vehicle with id {vehId} does not exist!");
    }

    [Fact]
    public async Task UpdateVehicleAsync_ShouldReturnUpdatedVehicle_WhenVehicleExistsWithSameFeature()
    {
        // Arrange
        var vehId = Guid.NewGuid();
        var vehicle = new Vehicle
        {
            VinNumber = "SOMEVIN",
            Announcement = new(),
            Owner = new(),
            Feature = new(),
            Id = vehId,
            Make = "Skoda",
            Model = "Fabia",
            Year = 2001,
            TechnicalState = State.Used,
            Mileage = 190000,
            PriceInEuro = 3000,
        };
        _unitOfWork.Vehicles.GetByIdAsync(Arg.Any<Guid>()).Returns(vehicle);
        _unitOfWork.Vehicles.GetVehicleAsync("SOMEVIN2").ReturnsNull();
        var upd = new UpdateVehicleDto("SOMEVIN2", "Skoda", "Model", 2001, 190000, "Used", 3000, "Hatchback", "Petrol", 1395, "Manual", 75);
        _unitOfWork.Features.GetFeatureAsync(Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<Gearbox>(), Arg.Any<Fuel>()).Returns(new Feature());

        // Act
        var veh = await _vehicleService.UpdateVehicleAsync(vehId, upd);

        // Assert
        veh.Should().NotBeNull();
        veh?.VinNumber.Should().Be("SOMEVIN2");
    }
}
