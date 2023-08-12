using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Mapping;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    ///     Register a new user
    /// </summary>
    /// <remarks>
    ///     Registers a new user with the provided information.
    /// </remarks>
    /// <param name="registerDto">The registration information</param>
    /// <returns>
    ///     201 - User registered successfully
    ///     - Content-Type: application/json
    ///     - Body: { "Id": "string", "Payload": RegisterDto }
    ///     409 - Conflict - User already exists
    ///     - Content-Type: text/plain
    ///     - Body: An user with the email '{registerDto.Email}' already exists
    ///     Default - Unexpected error
    ///     - Content-Type: application/json
    ///     - Body: Error
    /// </returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
    {
        var id = await _userService.RegisterUserAsync(registerDto);
        if (id != Guid.Empty)
        {
            var response = new
            {
                Id = id,
                Payload = registerDto
            };
            return Created($"{id}", response);
        }

        return Conflict($"An user with the email '{registerDto.Email}' already exists");
    }

    /// <summary>
    ///     Login User
    /// </summary>
    /// <remarks>
    ///     Authenticates a user by validating the provided login credentials.
    /// </remarks>
    /// <param name="loginDto">The login credentials</param>
    /// <returns>
    ///     200 - Successful authentication
    ///     - Content-Type: application/json
    ///     - Body: { "token": "string" }
    ///     404 - Not Found
    ///     - Content-Type: text/plain
    ///     - Body: Email or password was wrong
    /// </returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
    {
        var jwtToken = await _userService.LoginUserAsync(loginDto);

        if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrWhiteSpace(jwtToken))
            return NotFound("Email or password was wrong");

        return Ok(new { token = jwtToken });
    }

    /// <summary>
    ///     Get User by ID
    /// </summary>
    /// <remarks>
    ///     Retrieves user information by the provided user ID.
    ///     Does not require authorization.
    /// </remarks>
    /// <param name="id">The ID of the user to retrieve</param>
    /// <returns>
    ///     200 - Successful retrieval
    ///     - Content-Type: application/json
    ///     - Body: UserDto
    ///     404 - Not Found
    ///     - Content-Type: text/plain
    ///     - Body: User with ID {id} was not found!
    /// </returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user is null) return NotFound($"User with id {id} was not found!");
        return Ok(user.ToUserDto());
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = (await _userService.GetUsersAsync()).ToUserDto();

        return Ok(users);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto, [FromQuery] string originalEmail)
    {
        var user = await _userService.GetUserAsync(originalEmail);

        if (user is null) return NotFound();

        if (User.IsInRole("Admin") || User.HasClaim(ClaimTypes.NameIdentifier, user.Id.ToString()))
        {
            dto.Id = user.Id;
            user = await _userService.UpdateUserAsync(dto);
            return Ok(user);
        }

        return BadRequest();
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteUser([FromQuery] Guid userId = default)
    {
        if (User.IsInRole("Seller") || userId == Guid.Empty)
            userId = Guid.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        var deleted = await _userService.DeleteUserAsync(userId);
        if (deleted is not null)
            return Ok(deleted);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}