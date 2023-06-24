using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Mapping;

public static class VehicleMappingExtensions
{
    public static VehicleDto? ToVehicleDto(this Vehicle vehicle)
    {
        if (vehicle is null)
        {
            return null;
        }
        var vehicleDto = new VehicleDto()
        {
            VinNumber = vehicle.VinNumber,
            Year = vehicle.Year,
            Make = vehicle.Make,
            Model = vehicle.Model,
            CarBody = vehicle.Features.CarBody,
            Mileage = vehicle.Mileage,
            TechnicalState = vehicle.TechnicalState.ToString(),
            FuelType = vehicle.Features.FuelType.ToString(),
            Gearbox = vehicle.Features.Gearbox.ToString(),
            EngineSize = vehicle.Features.EngineSize,
            HorsePower = vehicle.Features.HorsePower,
            PriceInEuro = vehicle.PriceInEuro,
            PriceInRon = vehicle.PriceInRon,
            Owner = vehicle.Owner?.ToUserDto()
        };

        return vehicleDto;
    }

    public static List<VehicleDto?> ToListVehicleDto(this List<Vehicle> vehicle) 
    {
        var vehicles = new List<VehicleDto?>();
        foreach(var veh in vehicle)
        {
            vehicles.Add(veh.ToVehicleDto());
        }
        return vehicles;
    }

}