using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IVehicleRepository
{
    Task<List<Vehicle>> GetVehiclesAsync(VehicleFiltersDto? vehicleFilters);
    Task<Vehicle?> GetVehicleAsync(Guid id);
    Task<Vehicle?> GetVehicleAsync(string vinNumber);
    Task<Guid> InsertAsync(Vehicle vehicle);
    Task<Vehicle?> RemoveAsync(string vin);
    Task<Vehicle?> UpdateAsync(Vehicle vehicle);
    bool Any(Func<Vehicle, bool> predicate);
}