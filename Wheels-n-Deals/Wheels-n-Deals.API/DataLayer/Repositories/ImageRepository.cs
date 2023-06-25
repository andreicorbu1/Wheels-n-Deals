using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class ImageRepository : BaseRepository<Image>
{
    public ImageRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}