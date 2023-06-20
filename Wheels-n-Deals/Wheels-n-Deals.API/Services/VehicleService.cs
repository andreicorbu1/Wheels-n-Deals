using Microsoft.AspNetCore.JsonPatch;
using Wheels_n_Deals.API.DataLayer;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Mapping;

namespace Wheels_n_Deals.API.Services;

public class VehicleService
{
    private readonly UnitOfWork _unitOfWork;

    public VehicleService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> AddVehicle(AddVehicleDto addVehicleDto)
    {
        float price = CalculatePrice(addVehicleDto.Price, addVehicleDto.PriceCurrency);

        if (CheckExistingVehicleByVin(addVehicleDto.VinNumber))
        {
            return Guid.Empty;
        }

        var fuelType = (FuelType)Enum.Parse(typeof(FuelType), addVehicleDto.FuelType, true);
        var gearboxType = (GearboxType)Enum.Parse(typeof(GearboxType), addVehicleDto.Gearbox, true);
        var technicalState = (State)Enum.Parse(typeof(State), addVehicleDto.TechnicalState, true);

        Features feature = await GetOrCreateFeature(addVehicleDto, fuelType, gearboxType);
        if (feature == null)
        {
            return Guid.Empty;
        }

        User? owner = await _unitOfWork.Users.GetById(addVehicleDto.OwnerId);

        if (owner == null)
        {
            return Guid.Empty;
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
        return _unitOfWork.Vehicles.GetVehicleByVin(vinNumber).Result != null;
    }

    private async Task<Features> GetOrCreateFeature(AddVehicleDto addVehicleDto, FuelType fuelType, GearboxType gearboxType)
    {
        Features? feature = await _unitOfWork.Features.GetFeatureFromFeatures(addVehicleDto.CarBody,
            addVehicleDto.HorsePower, addVehicleDto.EngineSize, gearboxType, fuelType);

        if (feature == null)
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
        if (vehicle != null)
        {
            var result = await _unitOfWork.Vehicles.Remove(vehicle.Id) != null;
            await _unitOfWork.SaveChanges();
            return result;
        }
        return false;
    }

    public async Task<List<Vehicle>> GetAllVehicles()
    {
        return await _unitOfWork.Vehicles.GetAll();
    }

    public async Task<Vehicle?> GetVehicle(Guid id)
    {
        var vehicle = await _unitOfWork.Vehicles.GetById(id);

        return vehicle;
    }

    public async Task<VehicleDto?> GetVehicleFromVin(string vin)
    {
        var vehicle = await _unitOfWork.Vehicles.GetVehicleByVin(vin);

        if (vehicle == null)
        {
            return null;
        }

        return vehicle.ToVehicleDto();
    }

    public async Task<VehicleDto?> UpdateVehiclePatch(Guid id, JsonPatchDocument<Vehicle> vehiclePatch)
    {
        var vehicle = await _unitOfWork.Vehicles.UpdateVehiclePatch(id, vehiclePatch);

        if (vehicle == null)
        {
            return null;
        }
        await _unitOfWork.SaveChanges();
        return vehicle.ToVehicleDto();
    }

    public async Task<Vehicle?> UpdateVehicle(Vehicle updatedVehicle)
    {
        var vehicleToUpdate = await _unitOfWork.Vehicles.GetById(updatedVehicle.Id);

        if (vehicleToUpdate == null)
        {
            return null;
        }

        var features = updatedVehicle.Features;

        if (features != null)
        {
            var existingFeature = await _unitOfWork.Features.GetFeatureFromFeatures(
                features.CarBody, features.HorsePower, features.EngineSize, features.Gearbox, features.FuelType);

            if (existingFeature != null)
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
