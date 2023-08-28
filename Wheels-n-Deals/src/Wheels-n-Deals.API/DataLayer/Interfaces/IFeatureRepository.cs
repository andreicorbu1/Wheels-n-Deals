using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IFeatureRepository : IBaseRepository<Feature>
{
    Task<Feature?> GetFeatureAsync(string carBody, uint horsePower, uint engineSize, Gearbox gearbox, Fuel fuel);
}