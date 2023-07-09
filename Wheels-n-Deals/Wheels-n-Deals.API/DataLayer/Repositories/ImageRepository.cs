using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class ImageRepository : BaseRepository<Image>
{
    public ImageRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<Image>?> GetImagesOfAnnouncement(Guid announcementId)
    {
        return await GetRecords()
           .Where(im => im.AnnouncementId == announcementId)
           .ToListAsync();
    }
}