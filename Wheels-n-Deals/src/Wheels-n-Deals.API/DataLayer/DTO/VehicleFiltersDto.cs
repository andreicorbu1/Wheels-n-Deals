using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.DTO;

public class VehicleFiltersDto
{
    public List<string> Make { get; set; } = new();
    public List<string> Model { get; set; } = new();
    public uint? MinYear { get; set; }
    public uint? MaxYear { get; set; }
    public uint? MinMileage { get; set; }
    public uint? MaxMileage { get; set; }
    public float? MinPrice { get; set; }
    public float? MaxPrice { get; set; }
    public List<string> CarBody { get; set; } = new();
    public uint? MinEngineSize { get; set; }
    public uint? MaxEngineSize { get; set; }
    public List<Fuel> FuelType { get; set; } = new();
    public List<Gearbox> Gearbox { get; set; } = new();
    public uint? MinHorsePower { get; set; }
    public uint? MaxHorsePower { get; set; }
}