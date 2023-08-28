using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IImageRepository : IBaseRepository<Image>
{
    Task<Image?> GetImageAsync(string url);
}