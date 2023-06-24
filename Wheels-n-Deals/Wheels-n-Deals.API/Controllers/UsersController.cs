using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Mapping;
using Wheels_n_Deals.API.Services;

namespace Wheels_n_Deals.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private UserService UserService { get; set; }

    public UsersController(UserService userService)
    {
        UserService = userService;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <remarks>
    /// Registers a new user with the provided information.
    /// </remarks>
    /// <param name="registerDto">The registration information</param>
    /// <returns>
    /// 201 - User registered successfully
    ///   - Content-Type: application/json
    ///   - Body: { "Id": "string", "Payload": RegisterDto }
    /// 
    /// 409 - Conflict - User already exists
    ///   - Content-Type: text/plain
    ///   - Body: An user with the email '{registerDto.Email}' already exists
    ///
    /// Default - Unexpected error
    ///   - Content-Type: application/json
    ///   - Body: Error
    /// </returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
    {
        var id = await UserService.RegisterUser(registerDto);
        if (id != Guid.Empty)
        {
            var response = new
            {
                Id = id,
                Payload = registerDto
            };
            return Created($"{id}", response);
        }
        else
        {
            return Conflict($"An user with the email '{registerDto.Email}' already exists");
        }
    }

    /// <summary>
    /// Login User
    /// </summary>
    /// <remarks>
    /// Authenticates a user by validating the provided login credentials.
    /// </remarks>
    /// <param name="loginDto">The login credentials</param>
    /// <returns>
    /// 200 - Successful authentication
    ///   - Content-Type: application/json
    ///   - Body: { "token": "string" }
    ///
    /// 404 - Not Found
    ///   - Content-Type: text/plain
    ///   - Body: Email or password was wrong
    /// </returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
    {
        var jwtToken = await UserService.Validate(loginDto);

        if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrWhiteSpace(jwtToken))
            return NotFound("Email or password was wrong");

        return Ok(new { token = jwtToken });
    }

    /// <summary>
    /// Get User by ID
    /// </summary>
    /// <remarks>
    /// Retrieves user information by the provided user ID.
    /// Does not require authorization.
    /// </remarks>
    /// <param name="id">The ID of the user to retrieve</param>
    /// <returns>
    /// 200 - Successful retrieval
    ///   - Content-Type: application/json
    ///   - Body: UserDto
    ///
    /// 404 - Not Found
    ///   - Content-Type: text/plain
    ///   - Body: User with ID {id} was not found!
    /// </returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var user = await UserService.GetUserById(id);
        if (user is null) return NotFound($"User with id {id} was not found!");
        return Ok(user.ToUserDto());
    }

    [AllowAnonymous]
    [HttpGet()]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await UserService.GetUsersAsync();

        return Ok(users);
    }
}