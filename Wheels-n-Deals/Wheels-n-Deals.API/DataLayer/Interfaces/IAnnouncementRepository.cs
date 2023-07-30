using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IAnnouncementRepository
{
    Task<List<Announcement>> GetAnnouncementsAsync(List<Vehicle> vehicles);
    Task<Announcement?> GetAnnouncementAsync(Guid id);
    Task<Guid> InsertAsync(Announcement announcement);
    Task<Announcement?> RemoveAsync(Guid id);
    Task<Announcement?> UpdateAsync(Announcement announcement);
    bool Any(Func<Announcement, bool> predicate);
}
