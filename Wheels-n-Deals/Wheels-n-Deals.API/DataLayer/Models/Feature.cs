using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Models;

public class Feature
{
    public Guid Id { get; set; }
    [MaxLength(20)]
    public string CarBody { get; set; }
    public Fuel Fuel { get; set; }
    public uint EngineSize { get; set; }
    public Gearbox Gearbox { get; set; }
    public uint HorsePower { get; set; }

    // Relationships
    [JsonIgnore]
    public List<Vehicle> Vehicles { get; set; } = new();
}
