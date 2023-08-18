using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Vehicle?> GetVehicleAsync(string vinNumber)
    {
        if (_dbSet is null) return null;

        return await _dbSet.SingleOrDefaultAsync(v => v.VinNumber == vinNumber);
    }

    public async Task<Vehicle?> RemoveAsync(string vinNumber)
    {
        var vehicle = await GetVehicleAsync(vinNumber);

        if (vehicle is null) return null;

        _dbSet.Remove(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }
}