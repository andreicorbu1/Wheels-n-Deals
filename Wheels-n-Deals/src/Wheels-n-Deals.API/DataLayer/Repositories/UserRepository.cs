using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserAsync(string email)
    {
        if (_dbSet is null) return null;

        return await _dbSet.AsQueryable().FirstOrDefaultAsync(us => us.Email == email);
    }
}