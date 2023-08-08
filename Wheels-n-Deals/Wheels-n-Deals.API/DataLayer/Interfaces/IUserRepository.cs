using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(Guid id);
    Task<User?> GetUserAsync(string email);
    Task<Guid> InsertAsync(User user);
    Task<User?> RemoveAsync(Guid id);
    Task<User?> UpdateAsync(User user);
    bool Any(Func<User, bool> predicate);
}