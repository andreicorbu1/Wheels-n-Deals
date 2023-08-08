using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;
    private readonly IVehicleService _vehicleService;

    public AnnouncementController(IAnnouncementService announcementService, IVehicleService vehicleService)
    {
        _announcementService = announcementService;
        _vehicleService = vehicleService;
    }

    /// <summary>
    /// Get Announcement by ID
    /// </summary>
    /// <remarks>
    /// Retrieves announcement information by the provided announcement ID.
    /// </remarks>
    /// <param name="announcementId">The ID of the announcement to retrieve</param>
    /// <returns>
    /// 200 - Successful retrieval
    ///   - Content-Type: application/json
    ///   - Body: Announcement object
    ///
    /// 404 - Not Found
    ///   - Content-Type: text/plain
    ///   - Body: Announcement with ID {announcementId} was not found!
    /// </returns>
    [HttpGet("{announcementId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAnnouncementById([FromRoute] Guid announcementId)
    {
        var announcement = await _announcementService.GetAnnouncementAsync(announcementId);

        if (announcement is null)
            return NotFound($"Announcement with id {announcementId} was not found!");

        return Ok(announcement);
    }

    /// <summary>
    /// Add Announcement
    /// </summary>
    /// <remarks>
    /// Creates a new announcement with the provided information.
    /// </remarks>
    /// <param name="addAnnouncementDto"></param>
    /// <returns>
    /// 201 - Announcement created successfully
    ///   - Content-Type: application/json
    ///   - Body: { "Id": "string", "Payload": AnnouncementDto }
    ///
    /// 500 - Internal Server Error
    ///   - Content-Type: text/plain
    ///   - Body: Failed to create announcement.
    /// </returns>
    [Authorize]
    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> AddAnnouncement([FromBody] AddAnnouncementDto addAnnouncementDto)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return BadRequest();
        }
        addAnnouncementDto.UserId = Guid.Parse(userId.Value);
        var id = await _announcementService.AddAnnouncementAsync(addAnnouncementDto);
        if (id != Guid.Empty)
        {
            var response = new
            {
                Id = id,
                Payload = addAnnouncementDto
            };
            return Created($"{id}", response);
        }

        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create announcement.");
    }

    /// <summary>
    /// Update Announcement
    /// </summary>
    /// <remarks>
    /// Updates an existing announcement with the provided information.
    /// </remarks>
    /// <param name="announcementId">The ID of the announcement to update</param>
    /// <param name="updateDto">The updated announcement information</param>
    /// <returns>
    /// 200 - Successful update
    ///   - Content-Type: application/json
    ///   - Body: Updated Announcement object
    ///
    /// 404 - Not Found
    ///   - Content-Type: text/plain
    ///   - Body: Announcement with ID {id} was not found.
    /// </returns>
    [Authorize]
    [HttpPut("{announcementId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateAnnouncement([FromRoute] Guid announcementId, [FromBody] UpdateAnnouncementDto updateDto)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return BadRequest();
        }
        var updatedAnnouncement = await _announcementService.UpdateAnnouncementAsync(announcementId, updateDto);

        if (updatedAnnouncement is null)
        {
            return NotFound($"Announcement with ID {announcementId} was not found.");
        }

        return Ok(updatedAnnouncement);
    }

    /// <summary>
    /// Renew Announcement
    /// </summary>
    /// <remarks>
    /// Renews an existing announcement. Can be made once 24h.
    /// </remarks>
    /// <param name="announcementId">The ID of the announcement to update</param>
    /// <returns>
    /// 200 - Successful update
    ///   - Content-Type: application/json
    ///   - Body: Updated Announcement object
    ///
    /// 404 - Not Found
    ///   - Content-Type: text/plain
    ///   - Body: Announcement with ID {id} was not found.
    /// </returns>
    [Authorize]
    [HttpPut("renew/{announcementId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> RenewAnnouncement([FromRoute] Guid announcementId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return BadRequest();
        }
        var updatedAnnouncement = await _announcementService.RenewAnnouncementAsync(announcementId);

        if (updatedAnnouncement is null)
        {
            return NotFound($"Announcement with ID {announcementId} was not found.");
        }

        return Ok(updatedAnnouncement);
    }

    /// <summary>
    /// Delete Announcement
    /// </summary>
    /// <remarks>
    /// Deletes an existing announcement.
    /// </remarks>
    /// <param name="announcementId">The ID of the announcement to delete</param>
    /// <returns>
    /// 204 - Successful deletion
    ///
    /// 404 - Not Found
    ///   - Content-Type: text/plain
    ///   - Body: Announcement with ID {id} was not found.
    /// </returns>
    [Authorize]
    [HttpDelete("{announcementId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAnnouncement([FromRoute] Guid announcementId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return BadRequest();
        }
        var announcement = await _announcementService.GetAnnouncementAsync(announcementId);
        if (announcement is not null && announcement.Owner is not null && (User.IsInRole("Admin") || userId.Value == announcement.Owner.Id.ToString()))
        {
            var isDeleted = await _announcementService.DeleteAnnouncementAsync(announcementId);

            if (isDeleted is null)
            {
                return NotFound($"Announcement with ID {announcementId} was not found.");
            }

            return Ok(isDeleted);
        }
        return BadRequest();
    }

    /// <summary>
    /// Get All Announcements
    /// </summary>
    /// <remarks>
    /// Retrieves all the announcements.
    /// </remarks>
    /// <returns>
    /// 200 - Successful retrieval
    ///   - Content-Type: application/json
    ///   - Body: VehicleDto[]
    ///
    /// 500 - Unexpected error
    ///   - Content-Type: application/json
    ///   - Body: Error
    /// </returns>
    [HttpGet("getall")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Announcement>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAllAnnouncemenets([FromQuery] VehicleFiltersDto? vehicleFiltersDto)
    {
        var vehicles = await _vehicleService.GetVehiclesAsync(vehicleFiltersDto);
        var announcements = await _announcementService.GetAnnouncementsAsync(vehicles);

        return Ok(announcements);
    }

}
