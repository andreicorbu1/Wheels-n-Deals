using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class UserRepository : IUserRepository
{
    protected readonly AppDbContext _context;
    private readonly DbSet<User> _users;

    public UserRepository(AppDbContext context)
    {
        _context = context;
        _users = context.Set<User>();
    }

    public bool Any(Func<User, bool> predicate)
    {
        return _users.AsQueryable().Any(predicate);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        if (_users is null) return null;

        return await _users.AsQueryable().FirstOrDefaultAsync(us => us.Email == email);
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        if (_users is null) return null;

        return await _users.FindAsync(id);
    }

    public async Task<List<User>> GetUsersAsync()
    {
        if (_users is null) return null;

        return await _users.AsQueryable().ToListAsync();
    }

    public async Task<Guid> InsertAsync(User user)
    {
        if (_users is null) return Guid.Empty;

        await _users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<User> RemoveAsync(Guid id)
    {
        if (_users is null) return null;

        var user = await _users.FindAsync(id);

        if (user is null) throw new Exception("User not found in DB");

        _users.Remove(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        if (_users is null) return null;

        _context.ChangeTracker.Clear();
        _users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
