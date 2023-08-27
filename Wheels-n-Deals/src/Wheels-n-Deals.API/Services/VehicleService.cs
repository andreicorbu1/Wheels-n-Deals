using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.DataLayer.Utils;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.API.Services;

public class VehicleService : IVehicleService
{
    private readonly IUnitOfWork _unitOfWork;

    public VehicleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> AddVehicleAsync(AddVehicleDto addVehicleDto)
    {
        var fuelType = (Fuel)Enum.Parse(typeof(Fuel), addVehicleDto.FuelType, true);
        var gearboxType = (Gearbox)Enum.Parse(typeof(Gearbox), addVehicleDto.Gearbox, true);
        var technicalState = (State)Enum.Parse(typeof(State), addVehicleDto.TechnicalState, true);

        var feature = await GetOrCreateFeature(addVehicleDto, fuelType, gearboxType);

        var owner = await _unitOfWork.Users.GetByIdAsync(addVehicleDto.OwnerId);
        if (owner is null) return Guid.Empty;

        if (await GetVehicleAsync(addVehicleDto.VinNumber) != null)
            throw new ResourceExistingException($"A vehicle with the VIN {addVehicleDto.VinNumber} already exists!");

        var vehicle = new Vehicle
        {
            Make = addVehicleDto.Make,
            Model = addVehicleDto.Model,
            Year = addVehicleDto.Year,
            Mileage = addVehicleDto.Mileage,
            Feature = feature,
            Owner = owner,
            VinNumber = addVehicleDto.VinNumber,
            PriceInEuro = addVehicleDto.PriceInEuro,
            TechnicalState = technicalState
        };

        vehicle = await _unitOfWork.Vehicles.AddAsync(vehicle);

        if (vehicle is null) return Guid.Empty;

        feature?.Vehicles.Add(vehicle);
        owner.Vehicles.Add(vehicle);

        await _unitOfWork.SaveChangesAsync();

        return vehicle.Id;
    }

    public async Task<Vehicle?> DeleteVehicleAsync(string vin)
    {
        var result = await _unitOfWork.Vehicles.RemoveAsync(vin) ?? throw new ResourceMissingException($"Vehicle with VIN {vin} does not exist!");
        await _unitOfWork.SaveChangesAsync();
        return result;
    }

    public async Task<Vehicle?> GetVehicleAsync(string vin)
    {
        return await _unitOfWork.Vehicles.GetVehicleAsync(vin);
    }

    public async Task<Vehicle?> GetVehicleAsync(Guid id)
    {
        return await _unitOfWork.Vehicles.GetByIdAsync(id);
    }

    public async Task<List<Vehicle>> GetVehiclesAsync(VehicleFiltersDto? vehicleFilters)
    {
        return await _unitOfWork.Vehicles.GetManyAsync(VehicleExtensions.BuildFilterExpression(vehicleFilters)) ?? new();
    }

    public async Task<Vehicle?> UpdateVehicleAsync(Guid id, UpdateVehicleDto updatedVehicle)
    {
        var vehicleToUpdate = await _unitOfWork.Vehicles.GetByIdAsync(id) ??
                              throw new ResourceMissingException($"Vehicle with id {id} does not exist!");
        var vehicleWithNewVin = await _unitOfWork.Vehicles.GetVehicleAsync(updatedVehicle.VinNumber);
        if (vehicleWithNewVin is not null && vehicleWithNewVin.Id != id)
            throw new ResourceExistingException($"Vehicle with VIN {updatedVehicle.VinNumber} already exists!");

        var fuelType = (Fuel)Enum.Parse(typeof(Fuel), updatedVehicle.FuelType, true);
        var gearboxType = (Gearbox)Enum.Parse(typeof(Gearbox), updatedVehicle.Gearbox, true);
        var technicalState = (State)Enum.Parse(typeof(State), updatedVehicle.TechnicalState, true);

        var feature = await _unitOfWork.Features.GetFeatureAsync(updatedVehicle.CarBody, updatedVehicle.HorsePower,
            updatedVehicle.EngineSize, gearboxType, fuelType);
        if (feature is null)
        {
            feature = new Feature
            {
                CarBody = updatedVehicle.CarBody,
                HorsePower = updatedVehicle.HorsePower,
                EngineSize = updatedVehicle.EngineSize,
                Gearbox = gearboxType,
                Fuel = fuelType
            };
            await _unitOfWork.Features.AddAsync(feature);

        }
        vehicleToUpdate.Feature = feature;
        vehicleToUpdate.FeatureId = feature.Id;
        vehicleToUpdate.Year = updatedVehicle.Year;
        vehicleToUpdate.Make = updatedVehicle.Make;
        vehicleToUpdate.Model = updatedVehicle.Model;
        vehicleToUpdate.Mileage = updatedVehicle.Mileage;
        vehicleToUpdate.TechnicalState = technicalState;
        vehicleToUpdate.VinNumber = updatedVehicle.VinNumber;
        vehicleToUpdate.PriceInEuro = updatedVehicle.PriceInEuro;

        await _unitOfWork.Vehicles.UpdateAsync(vehicleToUpdate);
        await _unitOfWork.SaveChangesAsync();

        return vehicleToUpdate;
    }

    private async Task<Feature?> GetOrCreateFeature(AddVehicleDto addVehicleDto, Fuel fuelType, Gearbox gearboxType)
    {
        var feature = await _unitOfWork.Features.GetFeatureAsync(addVehicleDto.CarBody,
            addVehicleDto.HorsePower, addVehicleDto.EngineSize, gearboxType, fuelType);

        if (feature is null)
        {
            feature = new Feature
            {
                CarBody = addVehicleDto.CarBody,
                Gearbox = gearboxType,
                EngineSize = addVehicleDto.EngineSize,
                Fuel = fuelType,
                HorsePower = addVehicleDto.HorsePower
            };
            await _unitOfWork.Features.AddAsync(feature);
        }

        return feature;
    }

    public async Task<List<Vehicle>> GetUsersVehicles(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);

        if (user is null) throw new ResourceMissingException($"There is no user with the id {userId} in the database");
        
        var vehicles = await _unitOfWork.Vehicles.GetManyAsync(v => v.Owner != null && v.Owner.Id == userId);
        
        return vehicles ?? new();
    }
}