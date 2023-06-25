using Microsoft.AspNetCore.JsonPatch;
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

    public async Task<User?> GetUserById(Guid id)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> UpdateUserPatch(Guid id, JsonPatchDocument<User> userPatched)
    {
        var user = await GetUserById(id);

        if (user is null)
        {
            return null;
        }

        userPatched.ApplyTo(user);

        await Update(user);
        await _appDbContext.SaveChangesAsync();

        return user;
    }
}