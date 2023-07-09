using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class VehicleRepository : BaseRepository<Vehicle>
{
    public VehicleRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Vehicle?> GetVehicleByVin(string vinNumber)
    {
        var vehicles = GetRecords();

        var searchedVehicle = await vehicles.FirstOrDefaultAsync(v => v.VinNumber == vinNumber);

        return searchedVehicle;
    }

    public async Task<List<Vehicle?>> GetVehiclesByOwnerId(Guid ownerId)
    {
        var vehicles = GetRecords();

        var searchedVehicles = await vehicles.Where(v => v.Owner.Id == ownerId).ToListAsync();

        return searchedVehicles;
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

    public async Task<List<Vehicle>?> GetVehicles(VehicleFiltersDto? vehicleFilters)
    {

        var vehicles = GetRecords();

        if (vehicleFilters is null)
            return await vehicles.ToListAsync();

        if (!string.IsNullOrEmpty(vehicleFilters.Make))
            vehicles = vehicles
                .Where(v => v.Make == vehicleFilters.Make);

        if (!string.IsNullOrEmpty(vehicleFilters.Model))
            vehicles = vehicles
                .Where(v => v.Model == vehicleFilters.Model);

        if (!string.IsNullOrEmpty(vehicleFilters.CarBody))
            vehicles = vehicles
                .Where(v => v.Features.CarBody == vehicleFilters.CarBody);

        if (!string.IsNullOrEmpty(vehicleFilters.FuelType))
            vehicles = vehicles
                .Where(v => v.Features.FuelType.ToString() == vehicleFilters.FuelType);

        if (!string.IsNullOrEmpty(vehicleFilters.Gearbox))
            vehicles = vehicles
                .Where(v => v.Features.Gearbox.ToString() == vehicleFilters.Gearbox);

        if (vehicleFilters.MinYear is not null)
            vehicles = vehicles
                .Where(v => v.Year >= vehicleFilters.MinYear);

        if (vehicleFilters.MaxYear is not null)
            vehicles = vehicles
                .Where(v => v.Year <= vehicleFilters.MaxYear);

        if (vehicleFilters.MinMileage is not null)
            vehicles = vehicles
                .Where(v => v.Mileage >= vehicleFilters.MinMileage);

        if (vehicleFilters.MaxMileage is not null)
            vehicles = vehicles
                .Where(v => v.Mileage <= vehicleFilters.MaxMileage);

        if (vehicleFilters.MinPrice is not null)
            vehicles = vehicles
                .Where(v => v.PriceInEuro >= vehicleFilters.MinPrice);

        if (vehicleFilters.MaxPrice is not null)
            vehicles = vehicles
                .Where(v => v.PriceInEuro <= vehicleFilters.MaxPrice);


        if (vehicleFilters.MinEngineSize is not null)
            vehicles = vehicles
                .Where(v => v.Features.EngineSize >= vehicleFilters.MinEngineSize);

        if (vehicleFilters.MaxEngineSize is not null)
            vehicles = vehicles
                .Where(v => v.Features.EngineSize <= vehicleFilters.MaxEngineSize);

        if (vehicleFilters.MinHorsePower is not null)
            vehicles = vehicles
                .Where(v => v.Features.HorsePower >= vehicleFilters.MinHorsePower);

        if (vehicleFilters.MaxHorsePower is not null)
            vehicles = vehicles
                .Where(v => v.Features.HorsePower <= vehicleFilters.MaxHorsePower);


        return await vehicles.ToListAsync();
    }

    public async Task<bool> OnlyThisCarUsesFeature(Guid featureId)
    {
        var query = GetRecords();

        return await query.Where(v => v.Features.Id == featureId).CountAsync() == 1;
    }
}