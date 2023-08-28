using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class ImageRepository : BaseRepository<Image>, IImageRepository
{
    public ImageRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Image?> GetImageAsync(string url)
    {
        if (_dbSet is null) return null;

        return await _dbSet.FirstOrDefaultAsync(im => im.ImageUrl == url);
    }
}