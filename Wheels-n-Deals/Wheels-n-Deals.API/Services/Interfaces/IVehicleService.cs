using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.Services.Interfaces;

public interface IVehicleService
{
    Task<Guid> AddVehicleAsync(AddVehicleDto addVehicleDto);
    Task<Vehicle?> DeleteVehicleAsync(string vin);
    Task<List<Vehicle>> GetVehiclesAsync(VehicleFiltersDto? vehicleFilters);
    Task<Vehicle?> GetVehicleAsync(string vin);
    Task<Vehicle?> GetVehicleAsync(Guid id);
    Task<Vehicle?> UpdateVehicleAsync(Guid id, UpdateVehicleDto updatedVehicle);
}