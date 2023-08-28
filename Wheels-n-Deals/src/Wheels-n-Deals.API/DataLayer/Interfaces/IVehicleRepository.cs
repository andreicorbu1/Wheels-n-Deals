using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    Task<Vehicle?> GetVehicleAsync(string vinNumber);
    Task<Vehicle?> RemoveAsync(string vinNumber);
}