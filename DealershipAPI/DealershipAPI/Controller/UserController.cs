using DealershipAPI.Entity;
using DealershipAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DealershipAPI.Controller
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private UserService _userService;
		
		public UserController(UserService userService)
		{
			this._userService = userService;
		}

		[HttpGet("{userId}")]
		public IActionResult GetUserById(Guid userId)
		{
			var user = _userService.GetUser(userId);
			if(user == null)
			{
				return NotFound($"The user with the id {userId} was not found");
			}
			return Ok(user);
		}

		[HttpGet()]
		public IActionResult GetUsers()
		{
			return Ok(_userService.GetUsers());
		}

		[HttpPut]
		public IActionResult AddUser([FromBody]User user)
		{
			user.Id = Guid.NewGuid();
			_userService.AddUser(user);
			return Ok(user.Id);
		}

		[HttpDelete("{userId}")]
		public IActionResult DeleteUser([FromRoute]Guid userId)
		{
			var user = _userService.GetUser(userId);

			if(user == null)
			{
				return NotFound($"The user with the id {userId} was not found");
			}
			else
			{
				_userService.Delete(user);
			}
			return Ok(user);
		}
	}
}
