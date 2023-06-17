using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;
using Wheels_n_Deals.API.DataLayer.Mapping;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class VehicleRepository : BaseRepository<Vehicle>
{
    public VehicleRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Vehicle?> GetVehicleByVin(string vinNumber)
    {
        var vehicles = await GetAll();
        
        var searchedVehicle = vehicles.FirstOrDefault(v => v.VinNumber == vinNumber);
        
        return searchedVehicle;
    }
}