using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Mapping;

public static class VehicleMappingExtensions
{
    public static VehicleDto ToVehicleDto(this Vehicle vehicle, User owner, Features features)
    {
        var vehicleDto = new VehicleDto()
        {
            VinNumber = vehicle.VinNumber,
            Year = vehicle.Year,
            Make = vehicle.Make,
            Model = vehicle.Model,
            CarBody = features.CarBody,
            Mileage = vehicle.Mileage,
            TechnicalState = vehicle.TechnicalState.ToString(),
            FuelType = features.FuelType.ToString(),
            Gearbox = features.Gearbox.ToString(),
            EngineSize = features.EngineSize,
            HorsePower = features.HorsePower,
            PriceInEuro = vehicle.PriceInEuro,
            PriceInRon = vehicle.PriceInRon,
            Owner = owner.ToUserDto()
        };

        return vehicleDto;
    }
}