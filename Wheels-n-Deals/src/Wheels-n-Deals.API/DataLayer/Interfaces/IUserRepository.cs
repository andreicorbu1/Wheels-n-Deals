using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserAsync(string email);
}