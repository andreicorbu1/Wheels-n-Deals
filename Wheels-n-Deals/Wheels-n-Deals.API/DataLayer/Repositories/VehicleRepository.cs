using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Vehicle> _vehicles;

    public VehicleRepository(AppDbContext context)
    {
        _context = context;
        _vehicles = context.Vehicles;
    }

    public bool Any(Func<Vehicle, bool> predicate)
    {
        if (_vehicles is null) return false;

        return _vehicles.AsQueryable().Any(predicate);
    }

    public async Task<Vehicle?> GetVehicleAsync(Guid id)
    {
        if (_vehicles is null) return null;

        return await _vehicles.FindAsync(id);
    }

    public async Task<Vehicle?> GetVehicleAsync(string vinNumber)
    {
        if (_vehicles is null) return null;

        return await _vehicles.FirstOrDefaultAsync(v => v.VinNumber == vinNumber);
    }

    public async Task<List<Vehicle>> GetVehiclesAsync(VehicleFiltersDto? vehicleFilters)
    {
        if (_vehicles is null) return new List<Vehicle>();
        var vehicles = _vehicles.AsQueryable();

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
                .Where(v => v.Feature != null && v.Feature.CarBody == vehicleFilters.CarBody);

        if (!string.IsNullOrEmpty(vehicleFilters.FuelType))
            vehicles = vehicles
                .Where(v => v.Feature != null && v.Feature.Fuel.ToString() == vehicleFilters.FuelType);

        if (!string.IsNullOrEmpty(vehicleFilters.Gearbox))
            vehicles = vehicles
                .Where(v => v.Feature != null && v.Feature.Gearbox.ToString() == vehicleFilters.Gearbox);

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
                .Where(v => v.Feature != null && v.Feature.EngineSize >= vehicleFilters.MinEngineSize);

        if (vehicleFilters.MaxEngineSize is not null)
            vehicles = vehicles
                .Where(v => v.Feature != null && v.Feature.EngineSize <= vehicleFilters.MaxEngineSize);

        if (vehicleFilters.MinHorsePower is not null)
            vehicles = vehicles
                .Where(v => v.Feature != null && v.Feature.HorsePower >= vehicleFilters.MinHorsePower);

        if (vehicleFilters.MaxHorsePower is not null)
            vehicles = vehicles
                .Where(v => v.Feature != null && v.Feature.HorsePower <= vehicleFilters.MaxHorsePower);


        return await vehicles.ToListAsync();

    }

    public async Task<Guid> InsertAsync(Vehicle vehicle)
    {
        if (_vehicles is null) return Guid.Empty;

        await _vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        return vehicle.Id;
    }

    public async Task<Vehicle?> RemoveAsync(string vin)
    {
        if (_vehicles is null) return null;

        var vehicle = await _vehicles.FirstOrDefaultAsync(v => v.VinNumber == vin) ?? throw new ResourceMissingException($"Vehicle with vin {vin} not found in database!");
        _vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();

        return vehicle;
    }

    public async Task<Vehicle?> UpdateAsync(Vehicle vehicle)
    {
        if (_vehicles is null) return null;

        _context.ChangeTracker.Clear();
        _vehicles.Update(vehicle);
        await _context.SaveChangesAsync();

        return vehicle;
    }
}
