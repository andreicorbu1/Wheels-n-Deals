using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
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
        float price = CalculatePrice(addVehicleDto.Price, addVehicleDto.PriceCurrency);

        var fuelType = (Fuel)Enum.Parse(typeof(Fuel), addVehicleDto.FuelType, true);
        var gearboxType = (Gearbox)Enum.Parse(typeof(Gearbox), addVehicleDto.Gearbox, true);
        var technicalState = (State)Enum.Parse(typeof(State), addVehicleDto.TechnicalState, true);

        Feature? feature = await GetOrCreateFeature(addVehicleDto, fuelType, gearboxType);
        if (feature is null) return Guid.Empty;

        User? owner = await _unitOfWork.Users.GetUserAsync(addVehicleDto.OwnerId);
        if (owner is null) return Guid.Empty;

        if ((await GetVehicleAsync(addVehicleDto.VinNumber)) != null)
        {
            throw new ResourceExistingException($"A vehicle with the VIN {addVehicleDto.VinNumber} already exists!");
        }

        var vehicle = new Vehicle
        {
            Make = addVehicleDto.Make,
            Model = addVehicleDto.Model,
            Year = addVehicleDto.Year,
            Mileage = addVehicleDto.Mileage,
            Feature = feature,
            Owner = owner,
            VinNumber = addVehicleDto.VinNumber,
            PriceInEuro = price,
            TechnicalState = technicalState
        };

        var id = await _unitOfWork.Vehicles.InsertAsync(vehicle);

        feature.Vehicles.Add(vehicle);
        owner.Vehicles.Add(vehicle);
        await _unitOfWork.SaveChangesAsync();

        return id;
    }

    public async Task<Vehicle?> DeleteVehicleAsync(string vin)
    {
        var result = await _unitOfWork.Vehicles.RemoveAsync(vin);
        if (result is null) throw new ResourceMissingException($"Vehicle with VIN {vin} does not exist!");
        return result;
    }

    public async Task<Vehicle?> GetVehicleAsync(string vin)
    {
        return await _unitOfWork.Vehicles.GetVehicleAsync(vin) ?? throw new ResourceMissingException($"Vehicle with VIN {vin} does not exist!");
    }

    public async Task<Vehicle?> GetVehicleAsync(Guid id)
    {
        return await _unitOfWork.Vehicles.GetVehicleAsync(id) ?? throw new ResourceMissingException($"Vehicle with id {id} does not exist!");
    }

    public async Task<List<Vehicle>> GetVehiclesAsync(VehicleFiltersDto? vehicleFilters)
    {
        return await _unitOfWork.Vehicles.GetVehiclesAsync(vehicleFilters);
    }

    public async Task<Vehicle?> UpdateVehicleAsync(Guid id, UpdateVehicleDto updatedVehicle)
    {
        var vehicleToUpdate = await _unitOfWork.Vehicles.GetVehicleAsync(id) ?? throw new ResourceMissingException($"Vehicle with id {id} does not exist!");
        if ((await _unitOfWork.Vehicles.GetVehicleAsync(updatedVehicle.VinNumber))?.Id != id) throw new ResourceExistingException($"Vehicle with VIN {updatedVehicle.VinNumber} already exists!");

        var fuelType = (Fuel)Enum.Parse(typeof(Fuel), updatedVehicle.FuelType, true);
        var gearboxType = (Gearbox)Enum.Parse(typeof(Gearbox), updatedVehicle.Gearbox, true);
        var technicalState = (State)Enum.Parse(typeof(State), updatedVehicle.TechnicalState, true);

        var feature = await _unitOfWork.Features.GetFeatureAsync(updatedVehicle.CarBody, updatedVehicle.HorsePower, updatedVehicle.EngineSize, gearboxType, fuelType);
        if (feature is not null)
        {
            vehicleToUpdate.Feature = feature;
            vehicleToUpdate.FeatureId = feature.Id;
        }
        else
        {
            var featureId = await _unitOfWork.Features.InsertAsync(new Feature
            {
                CarBody = updatedVehicle.CarBody,
                HorsePower = updatedVehicle.HorsePower,
                EngineSize = updatedVehicle.EngineSize,
                Gearbox = gearboxType,
                Fuel = fuelType,
            });
            vehicleToUpdate.Feature ??= new();
            vehicleToUpdate.Feature.Id = featureId;
            vehicleToUpdate.Feature.CarBody = updatedVehicle.CarBody;
            vehicleToUpdate.Feature.Fuel = fuelType;
            vehicleToUpdate.Feature.Gearbox = gearboxType;
            vehicleToUpdate.Feature.EngineSize = updatedVehicle.EngineSize;
            vehicleToUpdate.Feature.HorsePower = updatedVehicle.HorsePower;
            vehicleToUpdate.FeatureId = featureId;
        }
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

    private static float CalculatePrice(float price, string priceCurrency)
    {
        if (priceCurrency == "RON")
        {
            return price / 5;
        }
        else
        {
            return price;
        }
    }

    private async Task<Feature?> GetOrCreateFeature(AddVehicleDto addVehicleDto, Fuel fuelType, Gearbox gearboxType)
    {
        Feature? feature = await _unitOfWork.Features.GetFeatureAsync(addVehicleDto.CarBody,
            addVehicleDto.HorsePower, addVehicleDto.EngineSize, gearboxType, fuelType);

        if (feature is null)
        {
            feature = new Feature()
            {
                CarBody = addVehicleDto.CarBody,
                Gearbox = gearboxType,
                EngineSize = addVehicleDto.EngineSize,
                Fuel = fuelType,
                HorsePower = addVehicleDto.HorsePower
            };
            await _unitOfWork.Features.InsertAsync(feature);
        }

        return feature;
    }
}
