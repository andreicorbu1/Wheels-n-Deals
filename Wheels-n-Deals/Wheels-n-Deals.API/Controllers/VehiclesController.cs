using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wheels_n_Deals.API.DataLayer.Dtos;
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
        Guid ownerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        if (User.IsInRole("Administrator") || User.IsInRole("Seller"))
        {
            addVehicleDto.OwnerId = ownerId;
            Guid vehicleId = await VehicleService.AddVehicle(addVehicleDto);
            if (vehicleId == Guid.Empty)
            {
                return Conflict("A vehicle with the same VIN already exists!");
            }

            return Created($"{vehicleId}", new { VehicleId = vehicleId, Vehicle = addVehicleDto });
        }

        return Unauthorized();
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
    [HttpGet("getall")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<VehicleDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAllVehicles()
    {
        var vehicles = await VehicleService.GetAllVehicles();

        return Ok(vehicles);
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

        if (vehicle != null)
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

        if (vehicle != null)
        {
            if (User.IsInRole("Administrator") ||
            (User.IsInRole("Seller") && vehicle?.Owner.Id == User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value))
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
}
