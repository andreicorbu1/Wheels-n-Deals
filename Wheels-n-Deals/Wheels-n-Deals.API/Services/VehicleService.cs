using Microsoft.AspNetCore.JsonPatch;
using Wheels_n_Deals.API.DataLayer;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Mapping;
using Wheels_n_Deals.API.Infrastructure.Exceptions;

namespace Wheels_n_Deals.API.Services;

public class VehicleService
{
    private readonly UnitOfWork _unitOfWork;
    private AnnouncementService _announcementService;
    public VehicleService(UnitOfWork unitOfWork, AnnouncementService announcementService)
    {
        _unitOfWork = unitOfWork;
        _announcementService = announcementService;
    }

    public async Task<Guid> AddVehicle(AddVehicleDto addVehicleDto)
    {
        float price = CalculatePrice(addVehicleDto.Price, addVehicleDto.PriceCurrency);

        if (CheckExistingVehicleByVin(addVehicleDto.VinNumber))
        {
            throw new ResourceExistingException($"Vehicle with id '{addVehicleDto.VinNumber}' is already existing!");
        }

        var fuelType = (FuelType)Enum.Parse(typeof(FuelType), addVehicleDto.FuelType, true);
        var gearboxType = (GearboxType)Enum.Parse(typeof(GearboxType), addVehicleDto.Gearbox, true);
        var technicalState = (State)Enum.Parse(typeof(State), addVehicleDto.TechnicalState, true);

        Features feature = await GetOrCreateFeature(addVehicleDto, fuelType, gearboxType);
        if (feature is null)
        {
            return Guid.Empty;
        }

        User? owner = await _unitOfWork.Users.GetById(addVehicleDto.OwnerId);

        if (owner is null)
        {
            throw new ResourceMissingException($"User with id '{addVehicleDto.OwnerId}' doesn't exist!");
        }

        var vehicle = new Vehicle
        {
            Make = addVehicleDto.Make,
            Model = addVehicleDto.Model,
            Year = addVehicleDto.Year,
            Mileage = addVehicleDto.Mileage,
            Features = feature,
            Owner = owner,
            VinNumber = addVehicleDto.VinNumber,
            PriceInEuro = price,
            TechnicalState = technicalState
        };

        var id = await _unitOfWork.Vehicles.Insert(vehicle) ?? Guid.Empty;
        await _unitOfWork.SaveChanges();
        return id;
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

    private bool CheckExistingVehicleByVin(string vinNumber)
    {
        return _unitOfWork.Vehicles.GetVehicleByVin(vinNumber).Result is not null;
    }

    private async Task<Features> GetOrCreateFeature(AddVehicleDto addVehicleDto, FuelType fuelType, GearboxType gearboxType)
    {
        Features? feature = await _unitOfWork.Features.GetFeatureFromFeatures(addVehicleDto.CarBody,
            addVehicleDto.HorsePower, addVehicleDto.EngineSize, gearboxType, fuelType);

        if (feature is null)
        {
            feature = new Features()
            {
                CarBody = addVehicleDto.CarBody,
                Gearbox = gearboxType,
                EngineSize = addVehicleDto.EngineSize,
                FuelType = fuelType,
                HorsePower = addVehicleDto.HorsePower
            };
            await _unitOfWork.Features.Insert(feature);
        }

        return feature;
    }

    public async Task<bool> DeleteVehicle(string vin)
    {
        var vehicle = await _unitOfWork.Vehicles.GetVehicleByVin(vin);
        
        if (vehicle is not null)
        {
            var announcement = await _unitOfWork.Announcements.GetAnnouncementFromVehicle(vin);
            if (announcement is not null)
            {
                await _announcementService.DeleteAnnouncement(announcement.User.Id, announcement.Id);
            }
            var featuresId = vehicle.Features.Id;
            if (await _unitOfWork.Vehicles.OnlyThisCarUsesFeature(featuresId))
            {
                await _unitOfWork.Features.Remove(featuresId);
            }
            var result = await _unitOfWork.Vehicles.Remove(vehicle.Id) is not null;
            await _unitOfWork.SaveChanges();
            return result;
        }

        return false;
    }

    public async Task<List<Vehicle>> GetVehicles(VehicleFiltersDto? vehicleFilters)
    {
        return await _unitOfWork.Vehicles.GetVehicles(vehicleFilters); 
    }

    public async Task<Vehicle?> GetVehicle(Guid id)
    {
        var vehicle = await _unitOfWork.Vehicles.GetById(id);

        return vehicle;
    }

    public async Task<VehicleDto?> GetVehicleFromVin(string vin)
    {
        var vehicle = await _unitOfWork.Vehicles.GetVehicleByVin(vin);

        if (vehicle is null)
        {
            throw new ResourceMissingException($"Vehicle with VIN '{vin}' doesn't exist!");
        }

        return vehicle.ToVehicleDto();
    }

    public async Task<VehicleDto?> UpdateVehiclePatch(Guid id, JsonPatchDocument<Vehicle> vehiclePatch)
    {
        var vehicle = await _unitOfWork.Vehicles.UpdateVehiclePatch(id, vehiclePatch);

        if (vehicle is null)
        {
            throw new ResourceMissingException($"Vehicle with id '{id}' doesn't exist!");
        }
        await _unitOfWork.SaveChanges();
        return vehicle.ToVehicleDto();
    }

    public async Task<Vehicle?> UpdateVehicle(Vehicle updatedVehicle)
    {
        var vehicleToUpdate = await _unitOfWork.Vehicles.GetById(updatedVehicle.Id);

        if (vehicleToUpdate is null)
        {
            throw new ResourceMissingException($"Vehicle with id '{updatedVehicle.Id}' doesn't exist!");
        }

        var features = updatedVehicle.Features;

        if (features is not null)
        {
            var existingFeature = await _unitOfWork.Features.GetFeatureFromFeatures(
                features.CarBody, features.HorsePower, features.EngineSize, features.Gearbox, features.FuelType);

            if (existingFeature is not null)
            {
                updatedVehicle.Features = existingFeature;
            }
            else
            {
                var id = await _unitOfWork.Features.Insert(new Features
                {
                    CarBody = features.CarBody,
                    EngineSize = features.EngineSize,
                    FuelType = features.FuelType,
                    Gearbox = features.Gearbox,
                    HorsePower = features.HorsePower,
                });

                updatedVehicle.Features.Id = id.Value;
            }
        }

        await _unitOfWork.Vehicles.Update(updatedVehicle);
        await _unitOfWork.SaveChanges();

        return updatedVehicle;
    }
}
