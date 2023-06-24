using System.ComponentModel.DataAnnotations;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Entities;

public class Features : BaseEntity
{
    [MaxLength(50)] public string CarBody { get; set; } = string.Empty;
    public FuelType FuelType { get; set; }
    public uint EngineSize { get; set; }
    public GearboxType Gearbox { get; set; }
    public uint HorsePower { get; set; }
}

