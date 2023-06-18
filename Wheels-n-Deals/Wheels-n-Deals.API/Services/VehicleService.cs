using Microsoft.AspNetCore.Mvc;
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

        Guid featureId = await GetOrCreateFeature(addVehicleDto, fuelType, gearboxType);
        if (featureId == Guid.Empty)
        {
            return Guid.Empty;
        }

        var vehicle = CreateVehicleObject(addVehicleDto, price, featureId, technicalState);

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

    private async Task<Guid> GetOrCreateFeature(AddVehicleDto addVehicleDto, FuelType fuelType, GearboxType gearboxType)
    {
        Guid featureId = await _unitOfWork.Features.GetFeatureIdFromFeatures(addVehicleDto.CarBody,
            addVehicleDto.HorsePower, addVehicleDto.EngineSize, gearboxType, fuelType);

        if (featureId == Guid.Empty)
        {
            var feature = new Features()
            {
                CarBody = addVehicleDto.CarBody,
                Gearbox = gearboxType,
                EngineSize = addVehicleDto.EngineSize,
                FuelType = fuelType,
                HorsePower = addVehicleDto.HorsePower
            };
            featureId = await _unitOfWork.Features.Insert(feature) ?? Guid.Empty;
        }

        return featureId;
    }

    public async Task<bool> DeleteVehicle(string vin)
    {
        var vehicle = await _unitOfWork.Vehicles.GetVehicleByVin(vin);
        if(vehicle != null)
        {
            var result = await _unitOfWork.Vehicles.Remove(vehicle.Id) != null;
            await _unitOfWork.SaveChanges();
            return result;
        }
        return false;
    }

    private static Vehicle CreateVehicleObject(AddVehicleDto addVehicleDto, float price, Guid featureId, State technicalState)
    {
        var vehicle = new Vehicle()
        {
            Make = addVehicleDto.Make,
            Model = addVehicleDto.Model,
            Year = addVehicleDto.Year,
            Mileage = addVehicleDto.Mileage,
            PriceInEuro = price,
            VinNumber = addVehicleDto.VinNumber,
            FeatureId = featureId,
            Id = Guid.Empty,
            OwnerId = addVehicleDto.OwnerId,
            TechnicalState = technicalState
        };

        return vehicle;
    }

    public async Task<List<VehicleDto>> GetAllVehicles()
    {
        var vehicles = await _unitOfWork.Vehicles.GetAll();
        var vehicleDtos = new List<VehicleDto>();

        foreach (var vehicle in vehicles)
        {
            var (owner, features) = await GetOwnerAndFeaturesObject(vehicle);
            var vehicleDto = vehicle.ToVehicleDto(owner, features);
            vehicleDtos.Add(vehicleDto);
        }

        return vehicleDtos;
    }
    
    public async Task<VehicleDto> GetVehicleFromVin(string vin)
    {
        var vehicle = await _unitOfWork.Vehicles.GetVehicleByVin(vin);
        if(vehicle == null)
        {
            return null;
        }

        var (owner, features) = await GetOwnerAndFeaturesObject(vehicle);

        return vehicle.ToVehicleDto(owner, features);
    }

    private async Task<Tuple<User?, Features?>> GetOwnerAndFeaturesObject(Vehicle vehicle)
    {
        var owner = await _unitOfWork.Users.GetById(vehicle.OwnerId);
        var features = await _unitOfWork.Features.GetById(vehicle.FeatureId);

        return new Tuple<User?, Features?>(owner, features);
    }
}
