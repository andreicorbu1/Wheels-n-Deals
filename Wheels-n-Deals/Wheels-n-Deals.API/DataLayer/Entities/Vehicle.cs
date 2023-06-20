using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Entities;

public class Vehicle : BaseEntity
{
    [MaxLength(50)] public string VinNumber { get; set; } = string.Empty;
    [MaxLength(50)] public string Make { get; set; } = string.Empty;
    [MaxLength(50)] public string Model { get; set; } = string.Empty;
    public uint Year { get; set; }
    public uint Mileage { get; set; }
    public float PriceInEuro { get; set; }
    [NotMapped] public float PriceInRon => (PriceInEuro * 5);
    public State TechnicalState { get; set; }
    public Features? Features { get; set; }
    public User? Owner { get; set; }
}
