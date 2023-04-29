using DealershipAPI.Entity;

namespace DealershipAPI.Repository;

public class UserRepository : BaseRepository<User>
{
	public UserRepository(AppDbContext context) : base(context) {}
}
