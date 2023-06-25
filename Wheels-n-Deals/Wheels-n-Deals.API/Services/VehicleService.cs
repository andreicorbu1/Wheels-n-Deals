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
        var announcements = await _unitOfWork.Announcements.GetAll();
        if (vehicle is not null)
        {
            if (announcements is not null)
            {
                var announcement = announcements.FirstOrDefault(a => a.Vehicle?.VinNumber == vin);
                if (announcement is not null)
                {
                    await _announcementService.DeleteAnnouncement(announcement.User.Id, announcement.Id);
                }
            }
            var featuresId = vehicle.Features.Id;
            if ((await _unitOfWork.Vehicles.GetAll()).Where(v => v.Features?.Id == featuresId).Count() == 1)
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
        var vehicles = await _unitOfWork.Vehicles.GetAll();

        if (vehicleFilters is null)
            return vehicles;

        if (!string.IsNullOrEmpty(vehicleFilters.Make))
            vehicles = vehicles
                .Where(v => v.Make == vehicleFilters.Make)
                .ToList();

        if (!string.IsNullOrEmpty(vehicleFilters.Model))
            vehicles = vehicles
                .Where(v => v.Model == vehicleFilters.Model)
                .ToList();

        if (!string.IsNullOrEmpty(vehicleFilters.CarBody))
            vehicles = vehicles
                .Where(v => v.Features?.CarBody == vehicleFilters.CarBody)
                .ToList();

        if (!string.IsNullOrEmpty(vehicleFilters.FuelType))
            vehicles = vehicles
                .Where(v => v.Features?.FuelType.ToString() == vehicleFilters.FuelType)
                .ToList();

        if (!string.IsNullOrEmpty(vehicleFilters.Gearbox))
            vehicles = vehicles
                .Where(v => v.Features?.Gearbox.ToString() == vehicleFilters.Gearbox)
                .ToList();

        if (vehicleFilters.MinYear is not null)
            vehicles = vehicles
                .Where(v => v.Year >= vehicleFilters.MinYear)
                .ToList();

        if (vehicleFilters.MaxYear is not null)
            vehicles = vehicles
                .Where(v => v.Year <= vehicleFilters.MaxYear)
                .ToList();

        if (vehicleFilters.MinMileage is not null)
            vehicles = vehicles
                .Where(v => v.Mileage >= vehicleFilters.MinMileage)
                .ToList();

        if (vehicleFilters.MaxMileage is not null)
            vehicles = vehicles
                .Where(v => v.Mileage <= vehicleFilters.MaxMileage)
                .ToList();

        if (vehicleFilters.MinPrice is not null)
            vehicles = vehicles
                .Where(v => v.PriceInEuro >= vehicleFilters.MinPrice)
                .ToList();

        if (vehicleFilters.MaxPrice is not null)
            vehicles = vehicles
                .Where(v => v.PriceInEuro <= vehicleFilters.MaxPrice)
                .ToList();


        if (vehicleFilters.MinEngineSize is not null)
            vehicles = vehicles
                .Where(v => v.Features?.EngineSize >= vehicleFilters.MinEngineSize)
                .ToList();

        if (vehicleFilters.MaxEngineSize is not null)
            vehicles = vehicles
                .Where(v => v.Features?.EngineSize <= vehicleFilters.MaxEngineSize)
                .ToList();

        if (vehicleFilters.MinHorsePower is not null)
            vehicles = vehicles
                .Where(v => v.Features?.HorsePower >= vehicleFilters.MinHorsePower)
                .ToList();

        if (vehicleFilters.MaxHorsePower is not null)
            vehicles = vehicles
                .Where(v => v.Features?.HorsePower <= vehicleFilters.MaxHorsePower)
                .ToList();


        return vehicles;
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
