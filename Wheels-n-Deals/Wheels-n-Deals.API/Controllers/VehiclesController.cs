using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Mapping;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    /// <summary>
    ///     Add Vehicle
    /// </summary>
    /// <remarks>
    ///     Adds a new vehicle to the system.
    ///     Requires authorization.
    /// </remarks>
    /// <param name="addVehicleDto">The vehicle information to add</param>
    /// <returns>
    ///     201 - Vehicle added successfully
    ///     - Content-Type: application/json
    ///     - Body: { "VehicleId": "string", "Vehicle": AddVehicleDto }
    ///     409 - Conflict - Vehicle already exists
    ///     - Content-Type: text/plain
    ///     - Body: A vehicle with the same VIN already exists!
    ///     401 - Unauthorized
    /// </returns>
    [HttpPost("add")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> AddVehicle([FromBody] AddVehicleDto addVehicleDto)
    {
        var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim is null) return Unauthorized();

        var ownerId = Guid.Parse(idClaim.Value);
        addVehicleDto.OwnerId = ownerId;

        var vehicleId = await _vehicleService.AddVehicleAsync(addVehicleDto);

        if (vehicleId == Guid.Empty) return Conflict("A vehicle with the same VIN already exists!");

        return Created($"{vehicleId}", new { VehicleId = vehicleId, Vehicle = addVehicleDto });
    }

    /// <summary>
    ///     Get All Vehicles
    /// </summary>
    /// <remarks>
    ///     Retrieves information for all vehicles.
    ///     No authentication required.
    /// </remarks>
    /// <returns>
    ///     200 - Successful retrieval
    ///     - Content-Type: application/json
    ///     - Body: VehicleDto[]
    ///     500 - Unexpected error
    ///     - Content-Type: application/json
    ///     - Body: Error
    /// </returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Vehicle>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAllVehicles([FromQuery] VehicleFiltersDto? vehicleFilters)
    {
        var vehicles = await _vehicleService.GetVehiclesAsync(vehicleFilters);

        return Ok(vehicles.ToVehicleDto());
    }

    /// <summary>
    ///     Get Vehicle from VIN
    /// </summary>
    /// <remarks>
    ///     Retrieves vehicle information by the provided VIN (Vehicle Identification Number).
    ///     No authentication required.
    /// </remarks>
    /// <param name="vin">The VIN of the vehicle to retrieve</param>
    /// <returns>
    ///     200 - Successful retrieval
    ///     - Content-Type: application/json
    ///     - Body: VehicleDto
    ///     404 - Not Found
    ///     500 - Unexpected error
    /// </returns>
    [HttpGet("{vin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleDto))]
    [ProducesDefaultResponseType]
    [AllowAnonymous]
    public async Task<IActionResult> GetVehicleFromVin([FromRoute] string vin)
    {
        var vehicle = await _vehicleService.GetVehicleAsync(vin);

        if (vehicle is not null)
            return Ok(vehicle);
        return NotFound();
    }

    /// <summary>
    ///     Delete Vehicles you own. Or if you are an administrator you can delete others vehicles.
    /// </summary>
    /// <remarks>
    ///     Deletes a vehicle by the provided VIN (Vehicle Identification Number).
    ///     Requires authorization.
    /// </remarks>
    /// <param name="vin">The VIN of the vehicle to delete</param>
    /// <returns>
    ///     200 - Vehicle deleted successfully
    ///     - Content-Type: application/json
    ///     - Body: VehicleDto
    ///     401 - Unauthorized
    ///     500 - Unexpected error
    ///     404 - Not Found
    /// </returns>
    [HttpDelete("{vin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [Authorize]
    public async Task<IActionResult> DeleteVehicle([FromRoute] string vin)
    {
        var vehicle = await _vehicleService.GetVehicleAsync(vin);

        if (vehicle is not null && vehicle.Owner is not null)
        {
            if (User.IsInRole("Admin") ||
                (User.IsInRole("User") && User.HasClaim(ClaimTypes.NameIdentifier, vehicle.Owner.Id.ToString())))
            {
                var deleted = await _vehicleService.DeleteVehicleAsync(vin);
                if (deleted is not null)
                    return Ok(vehicle.ToVehicleDto());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Unauthorized();
        }

        return NotFound();
    }

    /// <summary>
    ///     Update Vehicle
    /// </summary>
    /// <remarks>
    ///     Updates a vehicle with the provided information.
    ///     Requires authorization.
    /// </remarks>
    /// <param name="id">Id of the vehicle to be updated</param>
    /// <param name="updatedVehicle">The updated vehicle information</param>
    /// <returns>
    ///     200 - Vehicle updated successfully
    ///     - Content-Type: application/json
    ///     - Body: Vehicle
    ///     400 - Bad Request
    ///     401 - Unauthorized
    /// </returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Vehicle))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateVehicle([Required] Guid id, [FromBody] UpdateVehicleDto updatedVehicle)
    {
        var vehicle = await _vehicleService.GetVehicleAsync(id);

        if (vehicle is not null && vehicle.Owner is not null)
        {
            if (User.IsInRole("Admin") ||
                (User.IsInRole("User") && User.HasClaim(ClaimTypes.NameIdentifier, vehicle.Owner.Id.ToString())))
            {
                var vehicleToUpdate = await _vehicleService.UpdateVehicleAsync(id, updatedVehicle);

                if (vehicleToUpdate is null) return BadRequest();

                return Ok(vehicleToUpdate);
            }

            return Unauthorized();
        }

        return BadRequest();
    }
}