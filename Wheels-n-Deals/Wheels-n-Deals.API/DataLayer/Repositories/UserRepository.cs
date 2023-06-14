using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}