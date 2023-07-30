using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Announcement> _announcements;

    public AnnouncementRepository(AppDbContext context)
    {
        _context = context;
        _announcements = context.Announcements;
    }

    public bool Any(Func<Announcement, bool> predicate)
    {
        if (_announcements is null) return false;

        return _announcements.Any(predicate);
    }

    public async Task<Announcement?> GetAnnouncementAsync(Guid id)
    {
        if (_announcements is null) return null;

        return await _announcements.FindAsync(id);
    }

    public async Task<List<Announcement>> GetAnnouncementsAsync(List<Vehicle> vehicles)
    {
        var announcements = await _announcements.AsQueryable()
            .Where(ann => vehicles.Contains(ann.Vehicle))
            .ToListAsync();

        return announcements ?? new();
    }

    public async Task<Guid> InsertAsync(Announcement announcement)
    {
        if (_announcements is null) return Guid.Empty;

        await _announcements.AddAsync(announcement);
        await _context.SaveChangesAsync();

        return announcement.Id;
    }

    public async Task<Announcement?> RemoveAsync(Guid id)
    {
        if (_announcements is null) return null;

        var announcement = await _announcements.FindAsync(id) ?? throw new Exception("Announcement not found");

        _announcements.Remove(announcement);
        await _context.SaveChangesAsync();

        return announcement;
    }

    public async Task<Announcement?> UpdateAsync(Announcement announcement)
    {
        if (_announcements is null) return null;

        _context.ChangeTracker.Clear();
        _announcements.Update(announcement);
        await _context.SaveChangesAsync();

        return announcement;
    }
}
