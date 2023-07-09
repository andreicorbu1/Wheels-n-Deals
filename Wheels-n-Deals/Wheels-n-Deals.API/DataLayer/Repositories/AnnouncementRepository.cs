using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class AnnouncementRepository : BaseRepository<Announcement>
{
    public AnnouncementRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<Announcement>> GetAllAnnouncements(List<Vehicle> vehicles)
    {
        var announcements = await GetRecords()
            .Where(ann => vehicles.Contains(ann.Vehicle))
            .ToListAsync();

        return announcements;
    }

    public async Task<Announcement?> GetAnnouncementFromVehicle(string vin)
    {
        var announcements = GetRecords();

        var announcement = await announcements.FirstOrDefaultAsync(announcement => announcement.Vehicle.VinNumber == vin);

        return announcement;
    }
}