using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IFeatureRepository
{
    Task<List<Feature>> GetFeaturesAsync();
    Task<Feature?> GetFeatureAsync(Guid id);
    Task<Feature?> GetFeatureAsync(string carBody, uint horsePower, uint engineSize, Gearbox gearbox, Fuel fuel);
    Task<Guid> InsertAsync(Feature feature);
    Task<Feature?> RemoveAsync(Guid id);
    Task<Feature?> UpdateAsync(Feature feature);
    bool Any(Func<Feature, bool> predicate);
}