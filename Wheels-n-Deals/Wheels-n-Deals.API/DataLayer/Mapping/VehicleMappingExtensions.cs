using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Mapping;

public static class VehicleMappingExtensions
{
    public static VehicleDto ToVehicleDto(this Vehicle vehicle)
    {
        if (vehicle is null || vehicle.Feature is null) throw new ArgumentNullException(nameof(vehicle));
        var vehicleDto = new VehicleDto(vehicle.Id, vehicle.VinNumber, vehicle.Make, vehicle.Model, vehicle.Year,
            vehicle.Mileage, vehicle.TechnicalState.ToString(), vehicle.PriceInEuro, vehicle.PriceInRon,
            vehicle.Feature.CarBody,
            vehicle.Feature.Fuel.ToString(), vehicle.Feature.EngineSize, vehicle.Feature.Gearbox.ToString(),
            vehicle.Feature.HorsePower);
        return vehicleDto;
    }

    public static List<VehicleDto> ToVehicleDto(this List<Vehicle> vehicles)
    {
        return (from vehicle in vehicles
            let vehicleDto = vehicle.ToVehicleDto()
            select vehicleDto).ToList();
    }
}