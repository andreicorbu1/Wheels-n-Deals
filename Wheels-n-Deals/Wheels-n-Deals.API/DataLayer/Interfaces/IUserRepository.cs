using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByEmailAsync(string email);
    Task<Guid> InsertAsync(User user);
    Task<User> RemoveAsync(Guid id);
    Task<User> UpdateAsync(User user);
    bool Any(Func<User, bool> predicate);
}
