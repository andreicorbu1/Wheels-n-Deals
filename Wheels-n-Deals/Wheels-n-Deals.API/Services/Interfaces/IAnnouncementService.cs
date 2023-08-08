using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.Services.Interfaces;

public interface IAnnouncementService
{
    Task<Guid> AddAnnouncementAsync(AddAnnouncementDto addAnnouncementDto);
    Task<Announcement?> DeleteAnnouncementAsync(Guid id);
    Task<List<Announcement>> GetAnnouncementsAsync(List<Vehicle> vehicles);
    Task<Announcement?> GetAnnouncementAsync(Guid id);
    Task<Announcement?> UpdateAnnouncementAsync(Guid id, UpdateAnnouncementDto updatedAnnouncement);
    Task<Announcement?> RenewAnnouncementAsync(Guid id);
}
