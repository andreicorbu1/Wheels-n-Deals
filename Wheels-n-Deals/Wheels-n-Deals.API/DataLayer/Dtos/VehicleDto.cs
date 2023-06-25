using System.ComponentModel.DataAnnotations;

namespace Wheels_n_Deals.API.DataLayer.Dtos;

public class VehicleDto
{
    public Guid Id { get; set; }
    [MaxLength(50)] public string VinNumber { get; set; } = string.Empty;
    [MaxLength(50)] public string Make { get; set; } = string.Empty;
    [MaxLength(50)] public string Model { get; set; } = string.Empty;
    public uint Year { get; set; }
    public uint Mileage { get; set; }
    public string TechnicalState { get; set; } = string.Empty;
    public float PriceInEuro { get; set; }
    public float PriceInRon { get; set; }
    [MaxLength(50)] public string CarBody { get; set; } = string.Empty;
    public string FuelType { get; set; } = string.Empty;
    public uint EngineSize { get; set; }
    public string Gearbox { get; set; } = string.Empty;
    public uint HorsePower { get; set; }
    public UserDto? Owner { get; set; }
}