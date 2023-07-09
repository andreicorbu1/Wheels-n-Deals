using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;
using Wheels_n_Deals.API.DataLayer.Mapping;
using Wheels_n_Deals.API.Services;

namespace Wheels_n_Deals.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VehiclesController : ControllerBase
{
    private VehicleService VehicleService { get; set; }

    public VehiclesController(VehicleService vehicleService)
    {
        VehicleService = vehicleService;
    }

    /// <summary>
    /// Add Vehicle
    /// </summary>
    /// <remarks>
    /// Adds a new vehicle to the system.
    /// Requires authorization.
    /// </remarks>
    /// <param name="addVehicleDto">The vehicle information to add</param>
    /// <returns>
    /// 201 - Vehicle added successfully
    ///   - Content-Type: application/json
    ///   - Body: { "VehicleId": "string", "Vehicle": AddVehicleDto }
    ///
    /// 409 - Conflict - Vehicle already exists
    ///   - Content-Type: text/plain
    ///   - Body: A vehicle with the same VIN already exists!
    ///
    /// 401 - Unauthorized
    /// </returns>
    [HttpPost("add")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(object))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> AddVehicle([FromBody] AddVehicleDto addVehicleDto)
    {
        if (!(User.IsInRole("Administrator") || User.IsInRole("User")))
        {
            return Unauthorized();
        }

        var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim is null)
        {
            return Unauthorized();
        }
        Guid ownerId = Guid.Parse(idClaim.Value);
        addVehicleDto.OwnerId = ownerId;

        Guid vehicleId = await VehicleService.AddVehicle(addVehicleDto);
        if (vehicleId == Guid.Empty)
        {
            return Conflict("A vehicle with the same VIN already exists!");
        }

        return Created($"{vehicleId}", new { VehicleId = vehicleId, Vehicle = addVehicleDto });
    }

    /// <summary>
    /// Get All Vehicles
    /// </summary>
    /// <remarks>
    /// Retrieves information for all vehicles.
    /// No authentication required.
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
    [HttpGet()]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Vehicle>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAllVehicles([FromQuery] VehicleFiltersDto? vehicleFilters)
    {
        var vehicles = await VehicleService.GetVehicles(vehicleFilters);

        return Ok(vehicles.ToListVehicleDto());
    }

    /// <summary>
    /// Get Vehicle from VIN
    /// </summary>
    /// <remarks>
    /// Retrieves vehicle information by the provided VIN (Vehicle Identification Number).
    /// No authentication required.
    /// </remarks>
    /// <param name="vin">The VIN of the vehicle to retrieve</param>
    /// <returns>
    /// 200 - Successful retrieval
    ///   - Content-Type: application/json
    ///   - Body: VehicleDto
    ///
    /// 404 - Not Found
    ///
    /// 500 - Unexpected error
    /// </returns>
    [HttpGet("{vin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleDto))]
    [ProducesDefaultResponseType]
    [AllowAnonymous]
    public async Task<IActionResult> GetVehicleFromVin([FromRoute] string vin)
    {
        var vehicle = await VehicleService.GetVehicleFromVin(vin);

        if (vehicle is not null)
        {
            return Ok(vehicle);
        }
        else
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Delete Vehicles you own. Or if you are an administrator you can delete others vehicles.
    /// </summary>
    /// <remarks>
    /// Deletes a vehicle by the provided VIN (Vehicle Identification Number).
    /// Requires authorization.
    /// </remarks>
    /// <param name="vin">The VIN of the vehicle to delete</param>
    /// <returns>
    /// 200 - Vehicle deleted successfully
    ///   - Content-Type: application/json
    ///   - Body: VehicleDto
    ///
    /// 401 - Unauthorized
    ///
    /// 500 - Unexpected error
    ///
    /// 404 - Not Found
    /// </returns>
    [HttpDelete("{vin}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [Authorize]
    public async Task<IActionResult> DeleteVehicle([FromRoute] string vin)
    {
        var vehicle = await VehicleService.GetVehicleFromVin(vin);

        if (vehicle is not null)
        {
            if (User.IsInRole("Administrator") ||
            (User.IsInRole("User") && vehicle?.Owner?.Id == User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value))
            {
                var deleted = await VehicleService.DeleteVehicle(vin);
                if (deleted)
                    return Ok(vehicle);
                else return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return Unauthorized();
            }
        }

        return NotFound();
    }

    /// <summary>
    /// Update Vehicle (Patch)
    /// </summary>
    /// <remarks>
    /// Updates a vehicle partially by applying a JSON Patch document.
    /// Requires authorization.
    /// </remarks>
    /// <param name="vehicleId">The ID of the vehicle to update</param>
    /// <param name="patchedVehicle">The JSON Patch document containing the partial updates</param>
    /// <returns>
    /// 200 - Vehicle updated successfully
    ///   - Content-Type: application/json
    ///   - Body: Vehicle
    ///
    /// 404 - Not Found
    ///   - Content-Type: text/plain
    ///   - Body: Vehicle with ID {vehicleId} was not found
    ///
    /// 401 - Unauthorized
    /// </returns>
    [HttpPatch("{vehicleId:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Vehicle))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateVehiclePatch([FromRoute] Guid vehicleId, [FromBody] JsonPatchDocument<Vehicle> patchedVehicle)
    {
        var vehicle = await VehicleService.GetVehicle(vehicleId);

        if (vehicle is not null)
        {
            if (User.IsInRole("Administrator") ||
            (User.IsInRole("User") && vehicle?.Owner?.Id.ToString() == User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value))
            {
                var updatedVehicle = await VehicleService.UpdateVehiclePatch(vehicleId, patchedVehicle);

                if (updatedVehicle is null)
                {
                    return NotFound("Vehicle with id vehicleId was not found");
                }

                return Ok(updatedVehicle);
            }
            else
            {
                return Unauthorized();
            }
        }

        return NotFound($"Vehicle with id {vehicleId} was not found");
    }

    /// <summary>
    /// Update Vehicle
    /// </summary>
    /// <remarks>
    /// Updates a vehicle with the provided information.
    /// Requires authorization.
    /// </remarks>
    /// <param name="updatedVehicle">The updated vehicle information</param>
    /// <returns>
    /// 200 - Vehicle updated successfully
    ///   - Content-Type: application/json
    ///   - Body: Vehicle
    ///
    /// 400 - Bad Request
    ///
    /// 401 - Unauthorized
    /// </returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Vehicle))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateVehicle([FromBody] Vehicle updatedVehicle)
    {
        var vehicle = await VehicleService.GetVehicle(updatedVehicle.Id);

        if (vehicle is not null)
        {
            if (User.IsInRole("Administrator") ||
            (User.IsInRole("User") && vehicle?.Owner?.Id.ToString() == User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value))
            {

                var vehicleToUpdate = await VehicleService.UpdateVehicle(updatedVehicle);

                if (vehicleToUpdate is null)
                {
                    return BadRequest();
                }

                return Ok(vehicleToUpdate);
            }
            else
                return Unauthorized();
        }

        return BadRequest();
    }
}
