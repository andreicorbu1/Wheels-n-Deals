using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IImageRepository
{
    Task<List<Image>> GetImagesAsync();
    Task<Image?> GetImageAsync(Guid id);
    Task<Image?> GetImageAsync(string url);
    Task<Guid> InsertAsync(Image image);
    Task<Image?> RemoveAsync(Guid id);
    Task<Image?> UpdateAsync(Image image);
    bool Any(Func<Image, bool> predicate);
}