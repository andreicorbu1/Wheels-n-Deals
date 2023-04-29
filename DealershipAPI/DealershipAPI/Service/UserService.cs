using DealershipAPI.Entity;
using DealershipAPI.Repository;

namespace DealershipAPI.Service;

public class UserService
{
	private UserRepository _userRepository;
	
	public UserService(UserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public User GetUser(Guid id)
	{
		return _userRepository.GetById(id);
	}

	public List<User> GetUsers()
	{
		return _userRepository.GetAll();
	}

	public void AddUser(User user)
	{
		_userRepository.Insert(user);
	}

	public void UpdateUser(User user)
	{
		_userRepository.Update(user);
	}

	public void Delete(User user)
	{
		_userRepository.Delete(user);
	}
}
