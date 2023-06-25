using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class AnnouncementRepository : BaseRepository<Announcement>
{
    public AnnouncementRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}