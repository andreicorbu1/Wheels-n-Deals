using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.Services;

namespace Wheels_n_Deals.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementController : ControllerBase
{
    private AnnouncementService AnnouncementService { get; set; }
    private VehicleService VehicleService { get; set; }

    public AnnouncementController(AnnouncementService announcementService, VehicleService vehicleService)
    {
        AnnouncementService = announcementService;
        VehicleService = vehicleService;
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
        var announcement = await AnnouncementService.GetAnnouncementById(announcementId);

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
        var id = await AnnouncementService.CreateAnnouncement(addAnnouncementDto);
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
        var updatedAnnouncement = await AnnouncementService.UpdateAnnouncement(Guid.Parse(userId.Value), announcementId, updateDto);

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
    public async Task<IActionResult> DeleteAnnouncement([FromRoute] Guid announcementId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return BadRequest();
        }

        var isDeleted = await AnnouncementService.DeleteAnnouncement(Guid.Parse(userId.Value), announcementId);

        if (!isDeleted)
        {
            return NotFound($"Announcement with ID {announcementId} was not found.");
        }

        return NoContent();
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AnnouncementDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAllAnnouncemenets([FromQuery] VehicleFiltersDto? vehicleFiltersDto)
    {
        var vehicles = await VehicleService.GetVehicles(vehicleFiltersDto);
        var announcements = await AnnouncementService.GetAllAnnouncements(vehicles);

        return Ok(announcements);
    }
}