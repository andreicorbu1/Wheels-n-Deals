using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class FeatureRepository : BaseRepository<Feature>, IFeatureRepository
{
    public FeatureRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Feature?> GetFeatureAsync(string carBody, uint horsePower, uint engineSize, Gearbox gearbox,
        Fuel fuel)
    {
        if (_dbSet is null) return null;

        return await _dbSet.SingleOrDefaultAsync(f =>
            f.CarBody == carBody &&
            f.Fuel == fuel &&
            f.HorsePower == horsePower &&
            f.EngineSize == engineSize &&
            f.Gearbox == gearbox
        );
    }
}