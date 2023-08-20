using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Wheels_n_Deals.API.DataLayer.DTO;

public class AddVehicleDto
{
    [Required][MaxLength(50)] public string VinNumber { get; set; } = string.Empty;
    [Required][MaxLength(50)] public string Make { get; set; } = string.Empty;
    [Required][MaxLength(50)] public string Model { get; set; } = string.Empty;
    [Required] public uint Year { get; set; }
    [Required] public uint Mileage { get; set; }
    [Required][MaxLength(50)] public string TechnicalState { get; set; } = string.Empty;
    [Required][MaxLength(20)] public string PriceCurrency { get; set; } = string.Empty;
    [Required] public float Price { get; set; }
    [Required][MaxLength(50)] public string CarBody { get; set; } = string.Empty;
    [Required] public uint EngineSize { get; set; }

    [Required][MaxLength(50)] public string FuelType { get; set; } = string.Empty;
    [Required][MaxLength(50)] public string Gearbox { get; set; } = string.Empty;
    [Required] public uint HorsePower { get; set; }
    [JsonIgnore] public Guid OwnerId { get; set; } = Guid.Empty;
}