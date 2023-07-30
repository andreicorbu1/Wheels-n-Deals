using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class FeatureRepository : IFeatureRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Feature> _features;

    public FeatureRepository(AppDbContext context)
    {
        _context = context;
        _features = context.Features;
    }

    public bool Any(Func<Feature, bool> predicate)
    {
        if (_features is null) return false;

        return _features.AsQueryable().Any(predicate);
    }

    public async Task<Feature?> GetFeatureAsync(Guid id)
    {
        if (_features is null) return null;

        return await _features.FindAsync(id);
    }

    public async Task<Feature?> GetFeatureAsync(string carBody, uint horsePower, uint engineSize, Gearbox gearbox, Fuel fuel)
    {
        if (_features is null) return null;

        return await _features.FirstOrDefaultAsync(f =>
            f.CarBody == carBody &&
            f.Fuel == fuel &&
            f.HorsePower == horsePower &&
            f.EngineSize == engineSize &&
            f.Gearbox == gearbox
            );
    }

    public async Task<List<Feature>> GetFeaturesAsync()
    {
        if (_features is null) return new List<Feature>();

        return await _features.ToListAsync();
    }

    public async Task<Guid> InsertAsync(Feature feature)
    {
        if (_features is null) return Guid.Empty;

        await _features.AddAsync(feature);
        await _context.SaveChangesAsync();

        return feature.Id;
    }

    public async Task<Feature?> RemoveAsync(Guid id)
    {
        if (_features is null) return null;

        var feature = await _features.FindAsync(id);
        if (feature is null) return null;

        _features.Remove(feature);
        await _context.SaveChangesAsync();

        return feature;
    }

    public async Task<Feature?> UpdateAsync(Feature feature)
    {
        if (_features is null) return null;

        _context.ChangeTracker.Clear();
        _features.Update(feature);
        await _context.SaveChangesAsync();

        return feature;
    }
}
