using Microsoft.AspNetCore.JsonPatch;
using Wheels_n_Deals.API.DataLayer.Entities;

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

    public async Task<Vehicle?> UpdateVehiclePatch(Guid id, JsonPatchDocument<Vehicle> vehiclePatched)
    {
        var vehicle = await GetById(id);

        if (vehicle is null)
        {
            return null;
        }

        vehiclePatched.ApplyTo(vehicle);

        await Update(vehicle);
        await _appDbContext.SaveChangesAsync();

        return vehicle;
    }
}