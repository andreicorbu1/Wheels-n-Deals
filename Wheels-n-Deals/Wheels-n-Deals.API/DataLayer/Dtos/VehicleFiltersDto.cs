using System.ComponentModel.DataAnnotations;

namespace Wheels_n_Deals.API.DataLayer.Dtos;

public class VehicleFiltersDto
{
    [MaxLength(50)] public string? Make { get; set; } = null;
    [MaxLength(50)] public string? Model { get; set; } = null;
    public uint? MinYear { get; set; } = null;
    public uint? MaxYear { get; set; } = (uint)DateTime.Now.Year;
    public uint? MinMileage { get; set; } = null;
    public uint? MaxMileage { get; set; } = null;
    public float? MinPrice { get; set; } = null;
    public float? MaxPrice { get; set; } = null;
    [MaxLength(50)] public string? CarBody { get; set; } = null;
    public uint? MinEngineSize { get; set; } = null;
    public uint? MaxEngineSize { get; set; } = null;

    [MaxLength(50)] public string? FuelType { get; set; } = null;
    [MaxLength(50)] public string? Gearbox { get; set; } = null;
    public uint? MinHorsePower { get; set; } = null;
    public uint? MaxHorsePower { get; set; } = null;
}
